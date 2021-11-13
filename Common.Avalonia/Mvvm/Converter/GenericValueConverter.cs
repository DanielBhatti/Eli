using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Common.Avalonia.Mvvm.Converter
{
    public abstract class GenericValueConverter<TSource, TTarget, TParameter> : IValueConverter
    {
        public abstract TTarget Convert(TSource value, Type targetType, TParameter parameter, CultureInfo culture);
        public abstract TSource ConvertBack(TTarget value, Type targetType, TParameter parameter, CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !(value is TSource))
                throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type. The passing type was {0} but the expected is {1}", value.GetType(), typeof(TSource)));
            if (parameter != null && !(parameter is TParameter))
                throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type as ConvertParameter. The passing type was {0} but the expected is {1}", parameter.GetType(), typeof(TParameter)));

            return Convert((TSource)value!, targetType, (TParameter)parameter!, culture)!;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !(value is TTarget))
                throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type. The passing type was {0} but the expected is {1}", value.GetType(), typeof(TTarget)));
            if (parameter != null && !(parameter is TParameter))
                throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type as ConvertParameter. The passing type was {0} but the expected is {1}", parameter.GetType(), typeof(TParameter)));

            return ConvertBack((TTarget)value!, targetType, (TParameter)parameter!, culture)!;
        }
    }

    public abstract class GenericValueConverter<TSource, TTarget> : GenericValueConverter<TSource, TTarget, object> { }

    public abstract class GenericValueConverter<TSourceTarget> : GenericValueConverter<TSourceTarget, TSourceTarget> { }
}
