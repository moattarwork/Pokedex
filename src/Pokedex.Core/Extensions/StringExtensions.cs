namespace Pokedex.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLineBreaks(this string input)
        {
            return input?.Replace("\n", " ").Replace("\f", " ");
        } 
    }
}