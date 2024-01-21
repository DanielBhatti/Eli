using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Eli.Avalonia.Converters;

public class CharToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value == null) return "";
        if(value is char c) return c.ToString();
        return value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value == null) return "";
        if(value is string s && s.Length == 1) return s[0];
        return value;
    }
}
