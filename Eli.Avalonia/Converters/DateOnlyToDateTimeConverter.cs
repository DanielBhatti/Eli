using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Eli.Avalonia.Converters;

public class DateOnlyToDateTimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is DateOnly dateOnly) return dateOnly.ToDateTime(TimeOnly.MinValue);
        else if(value is DateTime date) return date;
        else if(value is string s && DateTime.TryParse(s, out var sDateTime)) return sDateTime;
        else if(value != null && DateTime.TryParse(value.ToString(), culture, DateTimeStyles.None, out var oDateTime)) return oDateTime;
        else return DateTime.Now;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is DateTime dateTime) return DateOnly.FromDateTime(dateTime);
        if(value is DateOnly dateOnly) return dateOnly;
        if(value is string s && DateTime.TryParse(s, culture, DateTimeStyles.None, out var parsedDateTime)) return DateOnly.FromDateTime(parsedDateTime);
        if(value != null && DateTime.TryParse(value.ToString(), culture, DateTimeStyles.None, out var objParsedDateTime)) return DateOnly.FromDateTime(objParsedDateTime);
        else return DateOnly.FromDateTime(DateTime.Now);
    }
}
