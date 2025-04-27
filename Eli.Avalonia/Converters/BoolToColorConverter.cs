using System;
using System.Globalization;

namespace Eli.Avalonia.Converters;

public class BoolToColorConverter
{
    public string TrueColor { get; set; } = "Green";
    public string FalseColor { get; set; } = "Transparent";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is bool b && b) return TrueColor;
        else return FalseColor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

}
