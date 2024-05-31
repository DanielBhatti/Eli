using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Eli.Avalonia.Converter;

public abstract class GenericValueConverter<TSource, TTarget, TParameter> : IValueConverter
{
    public abstract TTarget Convert(TSource value, Type targetType, TParameter parameter, CultureInfo culture);
    public abstract TSource ConvertBack(TTarget value, Type targetType, TParameter parameter, CultureInfo culture);

    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not null and not TSource
            ? throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type. The passing type was {0} but the expected is {1}", value.GetType(), typeof(TSource)))
            : parameter is not null and not TParameter
            ? throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type as ConvertParameter. The passing type was {0} but the expected is {1}", parameter.GetType(), typeof(TParameter)))
            : Convert((TSource)value!, targetType, (TParameter)parameter!, culture) as object;
    }

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not null and not TTarget
            ? throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type. The passing type was {0} but the expected is {1}", value.GetType(), typeof(TTarget)))
            : parameter is not null and not TParameter
            ? throw new InvalidCastException(string.Format("In order to use the generic IValueConverter you have to use the correct type as ConvertParameter. The passing type was {0} but the expected is {1}", parameter.GetType(), typeof(TParameter)))
            : ConvertBack((TTarget)value!, targetType, (TParameter)parameter!, culture) as object;
    }
}

public abstract class GenericValueConverter<TSource, TTarget> : GenericValueConverter<TSource, TTarget, object> { }

public abstract class GenericValueConverter<TSourceTarget> : GenericValueConverter<TSourceTarget, TSourceTarget> { }
