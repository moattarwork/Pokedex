using System.Threading.Tasks;
using Refit;

namespace Pokedex.Core.Clients.Poke
{
    public interface IPokeApiClient
    {
        [Get("/pokemon-species/{name}")]
        Task<ExtendedPokemonSpecies> GetPokemonSpeciesAsync(string name);
    }
}