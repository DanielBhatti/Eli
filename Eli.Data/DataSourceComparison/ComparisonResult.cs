using Eli.Data.DataTypes;

namespace Eli.Data.DataSourceComparison;

public readonly record struct ComparisonResult
{
    public required string PrimaryKey { get; init; }
    public required string FieldName { get; init; }
    public required DataType? PredictedDataType { get; init; }
    public required ComparisonResultType ComparisonResultType { get; init; }
    public required object? LeftValue { get; init; }
    public required object? RightValue { get; init; }
}
