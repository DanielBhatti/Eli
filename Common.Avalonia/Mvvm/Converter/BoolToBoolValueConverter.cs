using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Common.Avalonia.Mvvm.Converter;

public class BoolToBoolValueConverter : AvaloniaObject, IValueConverter
{
    public static readonly DirectProperty<BoolToBoolValueConverter, bool> IsNegatedProperty = AvaloniaProperty.RegisterDirect<BoolToBoolValueConverter, bool>(
        nameof(IsNegated),
        ao => ao.IsNegated,
        (ao, v) => ao.IsNegated = v
        );

    private bool _isNegated = false;
    public bool IsNegated
    {
        get => _isNegated;
        set => SetAndRaise(IsNegatedProperty, ref _isNegated, value);
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value == null) return "";
        if(value.GetType() != typeof(bool)) return false;

        var v = (bool)value;
        return IsNegated ? !v : (object)v;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert(value, targetType, parameter, culture);
}
