using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pokedex.Core.Clients.FunTranslation;
using Pokedex.Core.Domain;

namespace Pokedex.Core.Translations
{
    public class ShakespeareTranslationStrategy : ITranslationStrategy
    {
        private readonly IFunTranslationClient _funTranslationClient;
        private readonly ILogger<ShakespeareTranslationStrategy> _logger;

        public ShakespeareTranslationStrategy(IFunTranslationClient funTranslationClient, ILogger<ShakespeareTranslationStrategy> logger)
        {
            _funTranslationClient = funTranslationClient ?? throw new ArgumentNullException(nameof(funTranslationClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public bool CanTranslate(PokemonInfo pokemonInfo)
        {
            if (pokemonInfo == null) throw new ArgumentNullException(nameof(pokemonInfo));
            
            return !(pokemonInfo.IsLegendary || "cave".Equals(pokemonInfo.Habitat, StringComparison.CurrentCulture));
        }

        public async Task<string> TranslateAsync(PokemonInfo pokemonInfo)
        {
            if (CanTranslate(pokemonInfo) == false)
                return pokemonInfo.Description;
            
            _logger.LogInformation($"Translating Pokemon {pokemonInfo.Name} description. Translation Mode: Shakespeare");
            
            var response = await _funTranslationClient.GetShakespeareTranslationAsync(new FunTranslationRequest
                {Text = pokemonInfo.Description});

            return response.Contents.Translated;
        }
    }
}