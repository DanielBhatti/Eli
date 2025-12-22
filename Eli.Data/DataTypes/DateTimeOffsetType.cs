using System.Globalization;

namespace Eli.Data.DataTypes;

public record class DateTimeOffsetType : DataType<DateTimeOffset>
{
    private static readonly IReadOnlyList<string> DefaultFormats = new[]
    {
        // ISO 8601 (offset required)
        "yyyy-MM-ddTHH:mmK",
        "yyyy-MM-ddTHH:mm:ssK",
        "yyyy-MM-ddTHH:mm:ss.FFFK",

        // Space-separated
        "yyyy-MM-dd HH:mm zzz",
        "yyyy-MM-dd HH:mm:ss zzz",
        "yyyy-MM-dd HH:mm:ss.FFF zzz",

        // RFC 3339 / common internet formats
        "yyyy-MM-dd'T'HH:mm:ss'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.FFF'Z'",

        // Compact ISO
        "yyyyMMddTHHmmK",
        "yyyyMMddTHHmmssK",

        // US
        "M/d/yyyy HH:mm zzz",
        "MM/dd/yyyy HH:mm zzz",
        "M/d/yyyy HH:mm:ss zzz",
        "MM/dd/yyyy HH:mm:ss zzz",

        // EU / international
        "d/M/yyyy HH:mm zzz",
        "dd/MM/yyyy HH:mm zzz",
        "d/M/yyyy HH:mm:ss zzz",
        "dd/MM/yyyy HH:mm:ss zzz",

        // Textual month
        "MMM d yyyy HH:mm zzz",
        "MMMM d yyyy HH:mm zzz",
        "d MMM yyyy HH:mm zzz",
        "d MMMM yyyy HH:mm zzz"
    };

    public IReadOnlyList<string> Formats { get; init; } = DefaultFormats;
    public IFormatProvider FormatProvider { get; init; } = CultureInfo.InvariantCulture;
    public DateTimeStyles DateTimeStyles { get; init; } = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
    public override DataTypeName Name => DataTypeName.DateTimeOffset;

    public override bool TryConvert(string value, out DateTimeOffset result)
    {
        result = default;
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        value = value.Trim();
        if(DateTimeOffset.TryParseExact(value, Formats.ToArray(), FormatProvider, DateTimeStyles, out result)) return true;
        return false;
    }

    public override RelationType Compare(DateTimeOffset left, DateTimeOffset right)
    {
        if(left < right) return RelationType.LessThan;
        else if(left > right) return RelationType.GreaterThan;
        else if(left == right) return RelationType.EqualTo;
        else return RelationType.NotEqualTo;
    }
}
