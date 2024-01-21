using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Eli.Avalonia.Mvvm.Converter;

public class EnumToNamesConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value == null) return Array.Empty<string>();
        var enumType = value.GetType();
        if(!enumType.IsEnum) return Array.Empty<string>();
        return Enum.GetNames(enumType);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
