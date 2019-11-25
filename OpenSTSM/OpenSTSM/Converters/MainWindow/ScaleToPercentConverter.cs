using System;
using System.Windows.Data;
using System.Globalization;

namespace OpenSTSM.Converters.MainWindow
{
    public class ScaleToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return (double)(int)((double)value * 100.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 100.0;
        }
    }
}
