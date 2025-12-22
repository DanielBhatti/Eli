using System.Globalization;

namespace Eli.Data.DataTypes;

public record class TimeType : DataType<TimeOnly>
{
    private static readonly IReadOnlyList<string> DefaultFormats = new[]
        {
        // 24-hour (ISO-like)
        "HH:mm",
        "HH:mm:ss",
        "HH:mm:ss.FFF",

        // Compact
        "HHmm",
        "HHmmss",

        // 12-hour with AM/PM
        "h:mm tt",
        "hh:mm tt",
        "h:mm:ss tt",
        "hh:mm:ss tt",

        // With seconds + fractional
        "h:mm:ss.FFF tt",
        "hh:mm:ss.FFF tt",

        // Time-only from ISO 8601 date-time
        "yyyy-MM-ddTHH:mm",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss.FFF",
        "yyyy-MM-ddTHH:mmK",
        "yyyy-MM-ddTHH:mm:ssK",
        "yyyy-MM-ddTHH:mm:ss.FFFK",

        // Space-separated date-time
        "yyyy-MM-dd HH:mm",
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss.FFF"
    };

    public IReadOnlyList<string> Formats { get; init; } = DefaultFormats;

    public IFormatProvider FormatProvider { get; init; } = CultureInfo.InvariantCulture;

    public DateTimeStyles DateTimeStyles { get; init; } = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal;

    public override DataTypeName Name => DataTypeName.Time;

    public override bool TryConvert(string value, out TimeOnly result)
    {
        result = default;
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        value = value.Trim();
        if(TimeOnly.TryParseExact(value, Formats.ToArray(), FormatProvider, DateTimeStyles, out result)) return true;
        if(DateTime.TryParseExact(value, Formats.ToArray(), FormatProvider, DateTimeStyles, out var dt))
        {
            result = TimeOnly.FromDateTime(dt);
            return true;
        }
        return false;
    }

    public override RelationType Compare(TimeOnly left, TimeOnly right)
    {
        if(left < right) return RelationType.LessThan;
        else if(left > right) return RelationType.GreaterThan;
        else if(left == right) return RelationType.EqualTo;
        else return RelationType.NotEqualTo;
    }
}
