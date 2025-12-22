using System.Globalization;

namespace Eli.Data.DataTypes;

public record class DateTimeType : DataType<DateTime>
{
    private static readonly IReadOnlyList<string> DefaultFormats =
    [
        // ISO 8601 (date only, normalized to midnight)
        "yyyy-MM-dd",
        "yyyyMMdd",

        // ISO 8601 (date-time)
        "yyyy-MM-ddTHH:mm",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss.FFF",
        "yyyy-MM-ddTHH:mmK",
        "yyyy-MM-ddTHH:mm:ssK",
        "yyyy-MM-ddTHH:mm:ss.FFFK",

        // Space-separated variants
        "yyyy-MM-dd HH:mm",
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss.FFF",

        // Common US
        "M/d/yyyy",
        "MM/dd/yyyy",
        "M/d/yyyy HH:mm",
        "MM/dd/yyyy HH:mm",
        "M/d/yyyy HH:mm:ss",
        "MM/dd/yyyy HH:mm:ss",

        // Common EU / international
        "d/M/yyyy",
        "dd/MM/yyyy",
        "d/M/yyyy HH:mm",
        "dd/MM/yyyy HH:mm",
        "d/M/yyyy HH:mm:ss",
        "dd/MM/yyyy HH:mm:ss",

        // Dotted / dashed variants
        "yyyy.MM.dd",
        "dd.MM.yyyy",
        "MM-dd-yyyy",
        "MM-dd-yyyy HH:mm",
        "MM-dd-yyyy HH:mm:ss",

        // Textual month
        "MMM d yyyy",
        "MMMM d yyyy",
        "d MMM yyyy",
        "d MMMM yyyy",
        "MMM d yyyy HH:mm",
        "MMMM d yyyy HH:mm",
        "d MMM yyyy HH:mm",
        "d MMMM yyyy HH:mm"
    ];

    public IReadOnlyList<string> Formats { get; init; } = DefaultFormats;
    public IFormatProvider FormatProvider { get; init; } = CultureInfo.InvariantCulture;
    public DateTimeStyles DateTimeStyles { get; init; } = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal;
    public override DataTypeName Name => DataTypeName.DateTime;

    public override bool TryConvert(string value, out DateTime result)
    {
        result = default;
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        value = value.Trim();
        if(DateTime.TryParseExact(value, Formats.ToArray(), FormatProvider, DateTimeStyles, out result)) return true;
        return false;
    }

    public override RelationType Compare(DateTime left, DateTime right)
    {
        if(left < right) return RelationType.LessThan;
        else if(left > right) return RelationType.GreaterThan;
        else if(left == right) return RelationType.EqualTo;
        else return RelationType.NotEqualTo;
    }
}
