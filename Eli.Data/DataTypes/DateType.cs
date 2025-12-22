using System.Globalization;

namespace Eli.Data.DataTypes;

public record class DateType : DataType<DateOnly>
{
    public static readonly IReadOnlyList<string> DefaultFormats =
    [
        // ISO 8601 (date)
        "yyyy-MM-dd",
        "yyyyMMdd",

        // ISO 8601 (date-time)
        "yyyy-MM-ddTHH:mm",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ss.FFF",
        "yyyy-MM-ddTHH:mmK",
        "yyyy-MM-ddTHH:mm:ssK",
        "yyyy-MM-ddTHH:mm:ss.FFFK",

        // Common US
        "M/d/yyyy",
        "MM/dd/yyyy",

        // Common EU / international
        "d/M/yyyy",
        "dd/MM/yyyy",

        // Dotted / dashed variants
        "yyyy.MM.dd",
        "dd.MM.yyyy",
        "MM-dd-yyyy",

        // Textual month
        "MMM d yyyy",
        "MMMM d yyyy",
        "d MMM yyyy",
        "d MMMM yyyy"
    ];

    public bool AllowDateTimeInputs { get; init; } = true;
    public IReadOnlyList<string> Formats { get; init; } = DefaultFormats;
    public IFormatProvider FormatProvider { get; init; } = CultureInfo.InvariantCulture;
    public DateTimeStyles DateTimeStyles { get; init; } = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal;
    public override DataTypeName Name => DataTypeName.Date;

    public override bool TryConvert(string value, out DateOnly result)
    {
        result = default;
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        value = value.Trim();
        if(DateOnly.TryParseExact(value, Formats.ToArray(), FormatProvider, DateTimeStyles, out result)) return true;

        if(AllowDateTimeInputs && DateTime.TryParseExact(value, Formats.ToArray(), FormatProvider, DateTimeStyles, out var dt))
        {
            result = DateOnly.FromDateTime(dt);
            return true;
        }
        return false;
    }

    public override RelationType Compare(DateOnly left, DateOnly right)
    {
        if(left < right) return RelationType.LessThan;
        else if(left > right) return RelationType.GreaterThan;
        else if(left == right) return RelationType.EqualTo;
        else return RelationType.NotEqualTo;
    }
}
