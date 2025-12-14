using Eli.Data.DataTypes;

namespace Eli.Data.Predictor;

public record class PredictorOptions
{
    public bool IgnoreCurrencySymbol { get; init; } = true;
    public IEnumerable<DataTypeName> PredictionPrecedence { get; init; } = PossiblePredictions;

    public static IEnumerable<DataTypeName> PossiblePredictions { get; } = new List<DataTypeName>() {
        DataTypeName.Boolean,
        DataTypeName.Character,
        DataTypeName.Date,
        DataTypeName.Time,
        DataTypeName.DateTime,
        DataTypeName.DateTimeOffset,
        DataTypeName.Integer,
        DataTypeName.Numeric,
        DataTypeName.Guid,
        DataTypeName.String,
        DataTypeName.Unknown,
    };
}