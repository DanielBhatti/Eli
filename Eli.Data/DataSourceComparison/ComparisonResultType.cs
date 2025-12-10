namespace Eli.Data.DataSourceComparison;

public enum ComparisonResultType
{
    NonComparable,
    LessThan,
    GreaterThan,
    EqualTo,
    NotEqualTo,
    LeftValueHeaderFieldMissing,
    RightValueHeaderFieldMissing,
    LeftValueNullOrEmpty,
    RightValueNullOrEmpty,
    MissingLeftRow,
    MissingRightRow,
    LeftDataTypeUnknown,
    RightDataTypeUnknown,
    BothDataTypesUnknown,
    DataTypeMismatch,
}

public static class ComparisonResultTypeExtensions
{
    public static RelationType FromComparisonResultType(this ComparisonResultType comparisonResultType) => comparisonResultType switch
    {
        ComparisonResultType.EqualTo => RelationType.EqualTo,
        ComparisonResultType.NotEqualTo => RelationType.NotEqualTo,
        ComparisonResultType.LessThan => RelationType.LessThan,
        ComparisonResultType.GreaterThan => RelationType.GreaterThan,
        _ => RelationType.NonComparable,
    };

    public static ComparisonResultType ToComparisonResultType(this RelationType relationType) => relationType switch
    {
        RelationType.EqualTo => ComparisonResultType.EqualTo,
        RelationType.NotEqualTo => ComparisonResultType.NotEqualTo,
        RelationType.LessThan => ComparisonResultType.LessThan,
        RelationType.GreaterThan => ComparisonResultType.GreaterThan,
        _ => ComparisonResultType.NonComparable,
    };
}
