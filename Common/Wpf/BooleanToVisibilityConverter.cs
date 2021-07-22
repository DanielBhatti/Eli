using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Wpf
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool NegateValue { get; set; } = false;
        public bool ConvertToHidden { get; set; } = false;

        public Visibility HiddenOrCollapsed
        { 
            get
            {
                if (ConvertToHidden) return Visibility.Hidden;
                else return Visibility.Collapsed;
            } 
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value.GetType() != typeof(bool)) return null;

            if ((bool)value == true)
            {
                if (!NegateValue) return (object)Visibility.Visible;
                else return (object)HiddenOrCollapsed;
            }
            else
            {
                if (!NegateValue) return (object)HiddenOrCollapsed;
                else return (object)Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value.GetType() != typeof(Visibility)) return null;

            if ((Visibility)value == Visibility.Visible)
            {
                if (!NegateValue) return (object)true;
                else return (object)false;
            }
            else
            {
                if (!NegateValue) return (object)false;
                else return (object)true;
            }
        }
    }
}
