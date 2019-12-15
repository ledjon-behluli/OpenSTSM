using System;
using System.Windows.Data;
using System.Globalization;
using OpenSTSM.Extensions;

namespace OpenSTSM.Converters
{
    public class InformationToControlElementNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var controlElementName = ((value as string).SplitStringOnChar('(')[0]).TrimEnd();
            return controlElementName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
