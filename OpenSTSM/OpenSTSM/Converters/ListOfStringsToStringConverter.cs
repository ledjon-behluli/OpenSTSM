using OpenSTSM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OpenSTSM.Converters
{
    [ValueConversion(typeof(List<string>), typeof(string))]
    public class ListOfStringsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = string.Join("", ((List<string>)value).ToArray());
            return $"[{result}]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = (string)value;
            input = string.Concat(input.RemoveChars('[', ']'));
            input = input.TrimStart().TrimEnd();
            return input.StringToListOfStrings();
        }
    }
}
