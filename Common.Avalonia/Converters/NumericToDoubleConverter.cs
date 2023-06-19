using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Common.Avalonia.Converters;

public class NumericToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value != null)
        {
            return System.Convert.ToDouble(value);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is double d)
        {
            return System.Convert.ChangeType(d, targetType);
        }
        return value;
    }
}
