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

        public static string SplitStringOnCharTakeFirst(this string input, char character)
        {
            List<string> data = input.SplitStringOnChar(character);
            if(data.Count > 0)
            {
                return data.FirstOrDefault();
            }

            return string.Empty;
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

        public static IEnumerable<string> RemoveEmpty(this IEnumerable<string> input)
        {
            if (input == null)
                throw new NullReferenceException();

            return input.Where(i => !(string.IsNullOrEmpty(i) || string.IsNullOrWhiteSpace(i)));
        }

        public static int CountNonEmpty(this IEnumerable<string> input)
        {
            var filtered = input.RemoveEmpty();
            return filtered != null ? filtered.Count() : 0;
        }

        public static string StringBetweenCharacters(this string input, char charFrom, char charTo)
        {
            int posFrom = input.IndexOf(charFrom);
            if (posFrom != -1) 
            {
                int posTo = input.IndexOf(charTo, posFrom + 1);
                if (posTo != -1)
                {
                    return input.Substring(posFrom + 1, posTo - posFrom - 1);
                }
            }

            return string.Empty;
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: 
                    throw new ArgumentNullException(nameof(input));
                case "": 
                    throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: 
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
