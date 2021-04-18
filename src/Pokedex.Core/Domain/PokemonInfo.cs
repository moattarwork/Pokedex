namespace Pokedex.Core.Domain
{
    public record PokemonInfo
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Habitat { get; init; }
        public bool IsLegendary { get; init; }
    }
}