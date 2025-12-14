using Accord.IO;
using Eli.Data.DataTypes;
using Eli.Text.Normalization;

namespace Eli.Data.Predictor;

public class DataTypePredictor
{
    public PredictorOptions Options { get; }
    public Normalizer Normalizer { get; }

    public DataTypePredictor() => (Options, Normalizer) = (new(), new());

    public DataTypePredictor(PredictorOptions options) => (Options, Normalizer) = (options, new());

    public DataTypePredictor(NormalizationConfiguration normalizationConfiguration) => (Options, Normalizer) = (new(), new(normalizationConfiguration));

    public DataTypePredictor(PredictorOptions options, NormalizationConfiguration normalizationConfiguration) => (Options, Normalizer) = (options, new(normalizationConfiguration));

    public DataType Predict(IEnumerable<string> values)
    {
        var nameToPredicted = PredictAll(values).ToDictionary(p => p.Name, p => p);
        foreach(var prediction in Options.PredictionPrecedence)
        {
            if(nameToPredicted.ContainsKey(prediction)) return nameToPredicted[prediction];
        }
        if(nameToPredicted.Count() > 0) return nameToPredicted.ElementAt(0).Value;
        else return new UnknownDataType();
    }

    public ISet<DataType> PredictAll(IEnumerable<string> values)
    {
        var isNullable = false;
        var isFixedLength = true;
        var maxLength = 0;
        var precision = 0;
        var scale = 0;
        var possiblePredictions = PredictorOptions.PossiblePredictions.ToHashSet();
        foreach(var value in values)
        {
            var normalized = Normalizer.Normalize(value);
            if(normalized.Length > maxLength)
            {
                if(maxLength != 0) isFixedLength = false;
                maxLength = normalized.Length;
            }

            if(string.IsNullOrEmpty(normalized)) isNullable = true;
            if(!PredictorRegex.CharRegex().IsMatch(normalized)) _ = possiblePredictions.Remove(DataTypeName.Character);
            if(!PredictorRegex.BooleanRegex().IsMatch(normalized)) _ = possiblePredictions.Remove(DataTypeName.Boolean);
            if(!PredictorRegex.IntegerRegex().IsMatch(normalized))
            {
                _ = possiblePredictions.Remove(DataTypeName.Integer);
                precision = Math.Max(precision, normalized.Length);
            }
            if(!PredictorRegex.NumericRegex().IsMatch(normalized))
            {
                _ = possiblePredictions.Remove(DataTypeName.Numeric);
                var split = normalized.Split('.');
                if(split.Length > 0) precision = Math.Max(precision, split[0].Length);
                if(split.Length > 1) scale = Math.Max(scale, split[1].Length);
            }
            if(!PredictorRegex.DateRegex().IsMatch(normalized)) _ = possiblePredictions.Remove(DataTypeName.Date);
            if(!PredictorRegex.TimeRegex().IsMatch(normalized)) _ = possiblePredictions.Remove(DataTypeName.Time);
            if(!PredictorRegex.DateTimeRegex().IsMatch(normalized)) _ = possiblePredictions.Remove(DataTypeName.DateTime);
            if(!PredictorRegex.DateTimeOffsetRegex().IsMatch(normalized)) _ = possiblePredictions.Remove(DataTypeName.DateTimeOffset);
            if(!PredictorRegex.GuidRegex().IsMatch(normalized)) _ = possiblePredictions.Remove(DataTypeName.Guid);
        }

        var predictions = new HashSet<DataType>();
        foreach(var possiblePrediction in possiblePredictions)
        {
            _ = predictions.Add(possiblePrediction switch
            {
                DataTypeName.Unknown => new UnknownDataType(),
                DataTypeName.Character => new CharType() { IsNullable = isNullable },
                DataTypeName.String => new StringType() { IsNullable = isNullable, MaxLength = maxLength, IsFixedLength = isFixedLength },
                DataTypeName.Boolean => throw new NotImplementedException(),
                DataTypeName.Integer => new IntegerType() { IsNullable = isNullable },
                DataTypeName.Numeric => new NumericType() { IsNullable = isNullable, Precision = precision, Scale = scale },
                DataTypeName.Date => throw new NotImplementedException(),
                DataTypeName.Time => throw new NotImplementedException(),
                DataTypeName.DateTime => throw new NotImplementedException(),
                DataTypeName.DateTimeOffset => throw new NotImplementedException(),
                DataTypeName.Guid => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            });
        }
        return predictions;
    }

    public DataType Predict(string value) => Predict(new[] { value });
}
