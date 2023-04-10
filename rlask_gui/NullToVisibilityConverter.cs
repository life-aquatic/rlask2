using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace rlask_gui
{
    // This helper class is used to make "Ei valittu" text disappear, when a customer is selected in combobox AddInvoiceWindow.xaml
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is null ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return true; }
    }
}
