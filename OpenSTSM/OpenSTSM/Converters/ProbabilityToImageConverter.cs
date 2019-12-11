using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenSTSM.Converters
{
    public class ProbabilityToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var probability = (decimal)value;

            if (probability > Settings.Default.LeafProbabilityThreshold)
            {
                return "/Resources/leaf-yellow.png";
            }
            else
            {
                return "/Resources/leaf-red.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
