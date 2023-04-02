using System;
using System.Globalization;
using System.Windows.Data;

namespace rlask_gui
{

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