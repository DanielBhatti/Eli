using Avalonia.Data.Converters;
using Eli.Text;
using System;
using System.Globalization;

namespace Eli.Avalonia.Converters;

internal class SpacedPascalCaseConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value?.ToString()?.ToSpacedPascalCase();

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
