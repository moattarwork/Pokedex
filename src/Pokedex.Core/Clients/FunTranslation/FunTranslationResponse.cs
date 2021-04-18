namespace Pokedex.Core.Clients.FunTranslation
{
    public sealed record FunTranslationResponse
    {
        public FunTranslationResult Success { get; init; }
        public FunTranslationContent Contents { get; init; }
    }
}