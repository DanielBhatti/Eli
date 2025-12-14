
namespace Eli.Data.DataTypes;

public record class StringType : DataType<string>
{
    public required bool IsFixedLength { get; init; }
    public required int MaxLength { get; init; }
    public StringComparison StringComparison { get; init; } = StringComparison.OrdinalIgnoreCase;

    public override DataTypeName Name => DataTypeName.String;

    public override bool TryConvert(string value, out string result)
    {
        result = "";
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        if(MaxLength > 0 && value.Length > MaxLength) return false;
        result = value;
        return true;
    }

    public override RelationType Compare(string left, string right)
    {
        if(left.Equals(right, StringComparison)) return RelationType.EqualTo;
        else return RelationType.NotEqualTo;
    }
}
