using System;

namespace Pokedex.Core
{
    public static class StringExtensions
    {
        public static string RemoveLineBreaks(this string input)
        {
            return input?.Replace("\n", " ").Replace("\f", " ");
        } 
    }
}