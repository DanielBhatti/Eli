using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Eli.Avalonia.Converters;

public class NumericToDoubleConverter : IValueConverter
{
    public static readonly NumericToDoubleConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is null) return double.NaN;

        try
        {
            return System.Convert.ToDouble(value, culture);
        }
        catch
        {
            return double.NaN;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is not double d)
            return AvaloniaProperty.UnsetValue;

        try
        {
            if(targetType == typeof(double) || targetType == typeof(double?))
                return d;
            if(targetType == typeof(float) || targetType == typeof(float?))
                return (float)d;
            if(targetType == typeof(int) || targetType == typeof(int?))
                return (int)Math.Round(d);
            if(targetType == typeof(long) || targetType == typeof(long?))
                return (long)Math.Round(d);
            if(targetType == typeof(decimal) || targetType == typeof(decimal?))
                return (decimal)d;
            if(targetType == typeof(short) || targetType == typeof(short?))
                return (short)Math.Round(d);
            if(targetType == typeof(byte) || targetType == typeof(byte?))
                return (byte)Math.Round(d);
        }
        catch
        {
            return AvaloniaProperty.UnsetValue;
        }

        return AvaloniaProperty.UnsetValue;
    }
}