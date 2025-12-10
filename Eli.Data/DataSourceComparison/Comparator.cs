using Eli.Data.DataSourceComparison.DataSources;
using Eli.Data.DataTypes;
using Eli.Data.Predictor;
using Eli.Text.Normalization;

namespace Eli.Data.DataSourceComparison;

public sealed class Comparator
{
    private Normalizer Normalizer { get; } = new();
    private DataTypePredictor DataTypePredictor { get; } = new();

    public List<ComparisonResult> Compare(DataSource leftDataSource, DataSource rightDataSource, string primaryKey) => Compare(leftDataSource, rightDataSource, [primaryKey]);

    private static string GetPrimaryKeyValue(IReadOnlyDictionary<string, string> fieldToValues, ICollection<string> primaryKeys) => string.Join("|", primaryKeys.Select(pk => fieldToValues[pk]));

    private static List<ValidationError> Validate(DataSource dataSource, ICollection<string> primaryKeys)
    {
        var errors = new List<ValidationError>();
        var missingPrimaryKeyRows = dataSource.FieldToValueCollection.Where(row => primaryKeys.Any(pk => !row.ContainsKey(pk))).ToList();

        if(missingPrimaryKeyRows.Count > 0) errors.Add(new() { Error = $"One or more rows in {dataSource.Name} are missing required primary key fields: {string.Join(", ", primaryKeys)}." });

        var duplicateKeys = dataSource.FieldToValueCollection.Select(row => GetPrimaryKeyValue(row, primaryKeys)).GroupBy(key => key).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

        if(duplicateKeys.Count > 0) errors.Add(new() { Error = $"Duplicate primary key combinations found in {dataSource.Name}: {string.Join(", ", duplicateKeys)}." });
        return errors;
    }

    public List<ComparisonResult> Compare(DataSource leftDataSource, DataSource rightDataSource, ICollection<string> primaryKeys)
    {
        var leftErrors = Validate(leftDataSource, primaryKeys);
        var rightErrors = Validate(rightDataSource, primaryKeys);
        if(leftErrors.Any() || rightErrors.Any())
        {
            var messageLines = new List<string> { "Could not complete comparison, the following errors occurred when trying to process the data:" };

            if(leftErrors.Any())
            {
                messageLines.Add($"Data Source {leftDataSource.Name}:");
                messageLines.AddRange(leftErrors.Select(e => $"  - {e}"));
            }

            if(rightErrors.Any())
            {
                messageLines.Add($"Data Source {rightDataSource.Name}:");
                messageLines.AddRange(rightErrors.Select(e => $"  - {e}"));
            }

            throw new Exception(string.Join(Environment.NewLine, messageLines));
        }

        var leftPkToFieldToValues = new Dictionary<string, IReadOnlyDictionary<string, string>>();
        var rightpkToFieldToValues = new Dictionary<string, IReadOnlyDictionary<string, string>>();

        var allFieldToValues = leftDataSource.FieldToValueCollection.SelectMany(row => row).Concat(rightDataSource.FieldToValueCollection.SelectMany(row => row)).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Value).ToList());

        foreach(var fieldToValue in leftDataSource.FieldToValueCollection)
        {
            var key = Normalizer.Normalize(GetPrimaryKeyValue(fieldToValue, primaryKeys));
            leftPkToFieldToValues.Add(key, fieldToValue);

            foreach(var field in fieldToValue.Keys) allFieldToValues[field].Add(fieldToValue[field]);
        }

        foreach(var fieldToValue in rightDataSource.FieldToValueCollection)
        {
            var key = Normalizer.Normalize(GetPrimaryKeyValue(fieldToValue, primaryKeys));
            rightpkToFieldToValues.Add(key, fieldToValue);

            foreach(var field in fieldToValue.Keys) allFieldToValues[field].Add(fieldToValue[field]);
        }

        var comparisonResults = new List<ComparisonResult>();
        var fieldToDataTypePrediction = allFieldToValues.ToDictionary(f => f.Key, f => DataTypePredictor.Predict(f.Value));
        foreach(var pk in leftPkToFieldToValues.Keys.Except(rightpkToFieldToValues.Keys))
        {
            comparisonResults.Add(new()
            {
                PrimaryKey = pk,
                PredictedDataType = null,
                ComparisonResultType = ComparisonResultType.MissingRightRow,
                LeftValue = pk,
                RightValue = null,
            });
        }

        foreach(var pk in rightpkToFieldToValues.Keys.Except(leftPkToFieldToValues.Keys))
        {
            comparisonResults.Add(new()
            {
                PrimaryKey = pk,
                PredictedDataType = null,
                ComparisonResultType = ComparisonResultType.MissingLeftRow,
                LeftValue = null,
                RightValue = pk,
            });
        }

        foreach(var pk in rightpkToFieldToValues.Keys.Intersect(leftPkToFieldToValues.Keys))
        {
            var leftFieldToValues = leftPkToFieldToValues[pk];
            var rightFieldToValues = rightpkToFieldToValues[pk];

            var fieldNames = leftFieldToValues.Keys.Union(rightFieldToValues.Keys);
            foreach(var fieldName in fieldNames)
            {
                var leftHasFieldName = leftFieldToValues.TryGetValue(fieldName, out var leftValue);
                var rightHasFieldName = rightFieldToValues.TryGetValue(fieldName, out var rightValue);
                if(leftHasFieldName && !rightHasFieldName)
                {
                    comparisonResults.Add(new()
                    {
                        PrimaryKey = pk,
                        PredictedDataType = null,
                        ComparisonResultType = ComparisonResultType.RightValueHeaderFieldMissing,
                        LeftValue = leftValue,
                        RightValue = null,
                    });
                    continue;
                }
                if(!leftHasFieldName && rightHasFieldName)
                {
                    comparisonResults.Add(new()
                    {
                        PrimaryKey = pk,
                        PredictedDataType = null,
                        ComparisonResultType = ComparisonResultType.LeftValueHeaderFieldMissing,
                        LeftValue = null,
                        RightValue = rightValue,
                    });
                    continue;
                }
                var dataTypePrediction = fieldToDataTypePrediction[fieldName];

                if(dataTypePrediction.Name == DataTypeName.Unknown)
                {
                    ComparisonResultType? comparisonResultType = null;
                    if(dataTypePrediction.Name == DataTypeName.Unknown) comparisonResultType = ComparisonResultType.LeftDataTypeUnknown;
                    else if(dataTypePrediction.Name != DataTypeName.Unknown) comparisonResultType = ComparisonResultType.RightDataTypeUnknown;
                    else comparisonResultType = ComparisonResultType.BothDataTypesUnknown;
                    comparisonResults.Add(new()
                    {
                        PrimaryKey = pk,
                        PredictedDataType = null,
                        ComparisonResultType = comparisonResultType.Value,
                        LeftValue = leftValue,
                        RightValue = rightValue,
                    });
                    continue;
                }

                var leftConverted = dataTypePrediction.TryConvert(leftValue ?? "", out var leftConvertedValue);
                var rightConverted = dataTypePrediction.TryConvert(rightValue ?? "", out var rightConvertedValue);
                var relationType = dataTypePrediction.Compare(leftConverted, rightConverted);

                comparisonResults.Add(new()
                {
                    PrimaryKey = pk,
                    PredictedDataType = dataTypePrediction,
                    ComparisonResultType = relationType.ToComparisonResultType(),
                    LeftValue = leftConverted,
                    RightValue = rightConverted,
                });
            }
        }

        return comparisonResults;
    }

    private record class ValidationError
    {
        public required string Error { get; init; }
    }
}
