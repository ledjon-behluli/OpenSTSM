using System;
using System.Collections.Generic;
using System.Windows.Data;
using OpenSTSM.Extensions;

namespace OpenSTSM.Converters
{
    [ValueConversion(typeof(List<string>), typeof(string))]
    public class ListOfDecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            var result = String.Join(" ", ((List<decimal>)value).ToArray());
            return $"[{result}]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = (string)value;
            input = string.Concat(input.RemoveChars('[', ']'));
            input = input.TrimStart().TrimEnd();
            List<decimal> coefficients = new List<decimal>();
            var coefficients_string = input.SplitStringOnChar(' ');
            coefficients_string.ForEach(c => coefficients.Add(decimal.Parse(c)));
            return coefficients;
        }
    }
}
