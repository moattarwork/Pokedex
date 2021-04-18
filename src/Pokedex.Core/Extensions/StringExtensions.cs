using System.Text.RegularExpressions;

namespace Pokedex.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLineBreaks(this string input)
        {
            return input?.Replace("\n", " ").Replace("\f", " ");
        }

        public static string NormalizeSpaces(this string input)
        {
            if (input == null)
                return null;
            
            var regex = new Regex("[ ]{2,}", RegexOptions.None);     
            return regex.Replace(input, " ");
        }
    }
}