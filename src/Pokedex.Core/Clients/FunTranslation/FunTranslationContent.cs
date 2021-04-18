namespace Pokedex.Core.Clients.FunTranslation
{
    public sealed record FunTranslationContent
    {
        public string Translated { get; init; }
        public string Text { get; init; }
        public string Translation { get; init; }
    }
}