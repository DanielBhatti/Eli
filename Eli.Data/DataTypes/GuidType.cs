namespace Eli.Data.DataTypes;

public record class GuidType : DataType<Guid>
{
    public static readonly IReadOnlyList<string> DefaultFormats =
    [
        // 32 digits
        "N", // 00000000000000000000000000000000
        // Hyphenated
        "D", // 00000000-0000-0000-0000-000000000000
        // Braced
        "B", // {00000000-0000-0000-0000-000000000000}
        // Parenthesized
        "P", // (00000000-0000-0000-0000-000000000000)
        // Hexadecimal (rare but supported)
        "X"  // {0x00000000,0x0000,0x0000,{0x00,...}}
    ];

    public IReadOnlyList<string> Formats { get; init; } = DefaultFormats;
    public override DataTypeName Name => DataTypeName.Guid;

    public override bool TryConvert(string value, out Guid result)
    {
        result = default;
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        value = value.Trim();
        foreach(var format in Formats) if(Guid.TryParseExact(value, format, out result)) return true;
        return false;
    }

    public override RelationType Compare(Guid left, Guid right)
    {
        if(left == right) return RelationType.EqualTo;
        return RelationType.NotEqualTo;
    }
}
