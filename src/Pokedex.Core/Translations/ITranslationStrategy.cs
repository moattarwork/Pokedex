using System.Threading.Tasks;
using Pokedex.Core.Domain;

namespace Pokedex.Core.Translations
{
    public interface ITranslationStrategy
    {
        bool CanTranslate(PokemonInfo pokemonInfo);
        Task<string> TranslateAsync(PokemonInfo pokemonInfo);
    }
}