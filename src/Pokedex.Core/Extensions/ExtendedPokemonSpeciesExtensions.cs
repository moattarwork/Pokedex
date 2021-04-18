using System;
using System.Linq;
using Pokedex.Core.Clients;
using Pokedex.Core.Domain;

namespace Pokedex.Core.Extensions
{
    public static class ExtendedPokemonSpeciesExtensions
    {
        public static PokemonInfo ToPokemonInfo(this ExtendedPokemonSpecies pokemonSpecies)
        {
            if (pokemonSpecies == null) throw new ArgumentNullException(nameof(pokemonSpecies));

            return new PokemonInfo
            {
                Name = pokemonSpecies.Name,
                Description =
                    pokemonSpecies.FlavorTextEntries?.FirstOrDefault(m => m.Language.Name == "en")?.FlavorText.RemoveLineBreaks(),
                Habitat = pokemonSpecies.Habitat?.Name,
                IsLegendary = pokemonSpecies.IsLegendary
            };
        }
    }
}