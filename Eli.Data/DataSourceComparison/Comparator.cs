using Eli.Data.DataSourceComparison.DataSources;
using Eli.Data.DataTypes;
using Eli.Data.Predictor;
using Eli.Text.Normalization;

namespace Eli.Data.DataSourceComparison;

public sealed class Comparator
{
    private Normalizer Normalizer { get; } = new();
    private DataTypePredictor DataTypePredictor { get; } = new();

    public List<ComparisonResult> Compare(DataSource left, DataSource right, string primaryKey) =>
        Compare(left, right, [primaryKey]);

    public List<ComparisonResult> Compare(DataSource left, DataSource right, IEnumerable<string> primaryKeys)
    {
        primaryKeys = primaryKeys.ToArray();

        var leftErrors = Validate(left, primaryKeys.ToList());
        var rightErrors = Validate(right, primaryKeys.ToList());

        if(leftErrors.Any() || rightErrors.Any())
        {
            var errors = leftErrors.Select(le => "Left DataSource: " + le.Error).ToList();
            errors.AddRange(rightErrors.Select(re => "Right DataSource: " + re.Error));
            throw new Exception(string.Join(Environment.NewLine, errors));
        }
        var leftByPk = IndexByNormalizedPk(left, primaryKeys.ToList());
        var rightByPk = IndexByNormalizedPk(right, primaryKeys.ToList());

        var fieldToPrediction = PredictFieldTypes(left, right);

        var results = new List<ComparisonResult>();

        var allPks = new HashSet<string>(leftByPk.Keys);
        allPks.UnionWith(rightByPk.Keys);

        foreach(var pk in allPks)
        {
            var leftExists = leftByPk.TryGetValue(pk, out var leftRow);
            var rightExists = rightByPk.TryGetValue(pk, out var rightRow);

            if(!leftExists)
            {
                results.Add(new ComparisonResult
                {
                    FieldName = string.Join(",", primaryKeys),
                    PrimaryKey = pk,
                    PredictedDataType = null,
                    ComparisonResultType = ComparisonResultType.MissingLeftRow,
                    LeftValue = null,
                    RightValue = pk,
                });
                continue;
            }

            if(!rightExists)
            {
                results.Add(new ComparisonResult
                {
                    FieldName = string.Join(",", primaryKeys),
                    PrimaryKey = pk,
                    PredictedDataType = null,
                    ComparisonResultType = ComparisonResultType.MissingRightRow,
                    LeftValue = pk,
                    RightValue = null,
                });
                continue;
            }

            var fields = leftRow!.Keys.Union(rightRow!.Keys);
            foreach(var field in fields)
            {
                var leftHas = leftRow.TryGetValue(field, out var leftRaw);
                var rightHas = rightRow.TryGetValue(field, out var rightRaw);

                if(leftHas && !rightHas)
                {
                    results.Add(new ComparisonResult
                    {
                        FieldName = field,
                        PrimaryKey = pk,
                        PredictedDataType = null,
                        ComparisonResultType = ComparisonResultType.RightValueHeaderFieldMissing,
                        LeftValue = leftRaw,
                        RightValue = null,
                    });
                    continue;
                }

                if(!leftHas && rightHas)
                {
                    results.Add(new ComparisonResult
                    {
                        FieldName = field,
                        PrimaryKey = pk,
                        PredictedDataType = null,
                        ComparisonResultType = ComparisonResultType.LeftValueHeaderFieldMissing,
                        LeftValue = null,
                        RightValue = rightRaw,
                    });
                    continue;
                }

                var prediction = fieldToPrediction[field];

                if(prediction.Name == DataTypeName.Unknown)
                {
                    results.Add(new ComparisonResult
                    {
                        FieldName = field,
                        PrimaryKey = pk,
                        PredictedDataType = null,
                        ComparisonResultType = ComparisonResultType.BothDataTypesUnknown,
                        LeftValue = leftRaw,
                        RightValue = rightRaw,
                    });
                    continue;
                }

                var leftOk = prediction.TryConvert(leftRaw ?? "", out var leftConverted);
                var rightOk = prediction.TryConvert(rightRaw ?? "", out var rightConverted);
                leftConverted = string.IsNullOrEmpty(leftRaw) ? null : leftConverted;
                rightConverted = string.IsNullOrEmpty(rightRaw) ? null : rightConverted;

                if(!leftOk || !rightOk)
                {
                    results.Add(new ComparisonResult
                    {
                        FieldName = field,
                        PrimaryKey = pk,
                        PredictedDataType = prediction,
                        ComparisonResultType = !leftOk && !rightOk
                            ? ComparisonResultType.BothDataTypesUnknown
                            : (!leftOk ? ComparisonResultType.LeftDataTypeUnknown : ComparisonResultType.RightDataTypeUnknown),
                        LeftValue = leftRaw,
                        RightValue = rightRaw,
                    });
                    continue;
                }

                var relation = leftConverted != null && rightConverted != null ? prediction.Compare(leftConverted, rightConverted) : RelationType.NonComparable;

                results.Add(new ComparisonResult
                {
                    FieldName = field,
                    PrimaryKey = pk,
                    PredictedDataType = prediction,
                    ComparisonResultType = relation.ToComparisonResultType(),
                    LeftValue = leftConverted,
                    RightValue = rightConverted,
                });
            }
        }
        return results;
    }

    private IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> IndexByNormalizedPk(
        DataSource dataSource,
        IReadOnlyCollection<string> primaryKeys)
    {
        var dict = new Dictionary<string, IReadOnlyDictionary<string, string>>();

        foreach(var row in dataSource.FieldToValueCollection)
        {
            var rawPk = GetPrimaryKeyValue(row, primaryKeys);
            var pk = Normalizer.Normalize(rawPk);
            dict.Add(pk, row);
        }

        return dict;
    }

    private IReadOnlyDictionary<string, DataType> PredictFieldTypes(DataSource left, DataSource right)
    {
        // One value collection per field; no double-counting.
        var valuesByField = left.FieldToValueCollection
            .Concat(right.FieldToValueCollection)
            .SelectMany(row => row)
            .GroupBy(kvp => kvp.Key)
            .ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Value).ToList());

        return valuesByField.ToDictionary(kvp => kvp.Key, kvp => DataTypePredictor.Predict(kvp.Value));
    }

    private static string GetPrimaryKeyValue(IReadOnlyDictionary<string, string> row, IEnumerable<string> primaryKeys) =>
        string.Join("```", primaryKeys.Select(pk => row[pk]));

    private static List<ValidationError> Validate(DataSource dataSource, IEnumerable<string> primaryKeys)
    {
        var pks = primaryKeys.ToArray();
        var errors = new List<ValidationError>();

        var missingPkRows = dataSource.FieldToValueCollection.Where(row => pks.Any(pk => !row.ContainsKey(pk))).ToList();
        if(missingPkRows.Count > 0)
        {
            errors.Add(new ValidationError
            {
                Error = $"One or more rows in {dataSource.Name} are missing required primary key fields: {string.Join(", ", pks)}."
            });
        }

        var duplicateKeys = dataSource.FieldToValueCollection
            .Select(row => GetPrimaryKeyValue(row, pks))
            .GroupBy(key => key)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if(duplicateKeys.Count > 0)
        {
            errors.Add(new ValidationError
            {
                Error = $"Duplicate primary key combinations found in {dataSource.Name}: {string.Join(", ", duplicateKeys)}."
            });
        }

        return errors;
    }

    private sealed record ValidationError
    {
        public required string Error { get; init; }
    }
}
