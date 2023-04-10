using System;
using System.Globalization;
using System.Windows.Data;

namespace rlask_gui
{
    // This helper class is used to convert number of days (int) into datetime and back,
    // in order to update DatePicker in AddInvoiceWindow.xaml when NumberInput is updated,
    // and update NumberInput, when DatePicker is updated
    [ValueConversion(typeof(int), typeof(DateTime))]
    public sealed class SpanToDateConerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => DateTime.Now.AddDays((int)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime d = (DateTime)value;
            return d > DateTime.Now ? d.Subtract(DateTime.Now).Days + 1 : DateTime.Now;
        }
    }
}