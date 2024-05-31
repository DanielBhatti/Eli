using Avalonia.Data.Converters;
using Eli.Avalonia.Mvvm;
using System;
using System.Globalization;

namespace Eli.Avalonia.Converter;

public class NotifyableValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value?.GetType() == targetType ? ((Notifyable)value).Value : "Could not convert.";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
