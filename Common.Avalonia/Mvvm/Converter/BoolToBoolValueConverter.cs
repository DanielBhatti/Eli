using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Avalonia.Mvvm.Converter
{
    public class BoolToBoolValueConverter : AvaloniaObject, IValueConverter
    {
        public static DirectProperty<BoolToBoolValueConverter, bool> IsNegatedProperty = AvaloniaProperty.RegisterDirect<BoolToBoolValueConverter, bool>(
            nameof(IsNegated),
            ao => ao.IsNegated,
            (ao, v) => ao.IsNegated = v
            );

        private bool _isNegated = false;
        public bool IsNegated
        {
            get => _isNegated;
            set => this.SetAndRaise(IsNegatedProperty, ref _isNegated, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(bool)) return false;

            bool v = (bool)value;
            if (IsNegated) return !v;
            else return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
