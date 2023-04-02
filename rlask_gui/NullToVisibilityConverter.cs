using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace rlask_gui
{

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is null ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return true; }
    }
}
