﻿using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Common.Avalonia.Mvvm.Converter
{
    public class NotifyableValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.GetType() == targetType) return ((Notifyable)value).Value;
            else return "Could not convert.";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
