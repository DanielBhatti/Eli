namespace Eli.Data.DataTypes;

public record class CharType : DataType<char>
{
    public override DataTypeName Name => DataTypeName.Character;
    public StringComparison StringComparison { get; init; } = StringComparison.OrdinalIgnoreCase;

    public override bool TryConvert(string value, out char result)
    {
        result = default;
        if(string.IsNullOrEmpty(value)) return IsNullable;
        var trimmed = value.Trim();
        if(trimmed.Length != 1) return false;
        result = trimmed[0];
        return true;
    }

    public override RelationType Compare(char left, char right)
    {
        if(left.ToString().Equals(right.ToString(), StringComparison)) return RelationType.EqualTo;
        else return RelationType.NotEqualTo;
    }
}
