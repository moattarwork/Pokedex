using Newtonsoft.Json;
using PokeApiNet;

namespace Pokedex.Core.Clients
{
    
    // NOTE: The original class is missing the property but it is coming back from the API
    public sealed class ExtendedPokemonSpecies : PokemonSpecies
    {
        /// <summary>Whether or not this is a legendary Pokémon.</summary>
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}