using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.Extensions
{
    public static class StringExtensions
    {
        public static List<string> SplitStringOnChar(this string input, char character)
        {
            return input.Split(character)
                        .Select(keyword => keyword.Trim())
                        .Where(keyword => !string.IsNullOrWhiteSpace(keyword)).ToList();
        }

        public static IEnumerable<char> RemoveChars(this IEnumerable<char> originalString, params char[] removingChars)
        {
            return originalString.ExceptAll(removingChars);
        }

        public static List<string> StringToListOfStrings(this string input)
        {
            List<string> datalist = new List<string>();
            datalist.AddRange(input.Select(c => c.ToString()));
            return datalist;
        }
    }
}
