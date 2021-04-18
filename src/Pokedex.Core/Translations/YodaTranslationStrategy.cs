using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pokedex.Core.Clients.FunTranslation;
using Pokedex.Core.Domain;

namespace Pokedex.Core.Translations
{
    public class YodaTranslationStrategy : ITranslationStrategy
    {
        private readonly IFunTranslationClient _funTranslationClient;
        private readonly ILogger<YodaTranslationStrategy> _logger;

        public YodaTranslationStrategy(IFunTranslationClient funTranslationClient, ILogger<YodaTranslationStrategy> logger)
        {
            _funTranslationClient = funTranslationClient ?? throw new ArgumentNullException(nameof(funTranslationClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public bool CanTranslate(PokemonInfo pokemonInfo)
        {
            if (pokemonInfo == null) throw new ArgumentNullException(nameof(pokemonInfo));
            
            return pokemonInfo.IsLegendary || "cave".Equals(pokemonInfo.Habitat, StringComparison.CurrentCulture);
        }

        public async Task<string> TranslateAsync(PokemonInfo pokemonInfo)
        {
            if (CanTranslate(pokemonInfo) == false)
                return pokemonInfo.Description;
            
            _logger.LogInformation($"Translating Pokemon {pokemonInfo.Name} description. Translation Mode: Yoda");

            var response = await _funTranslationClient.GetYodaTranslationAsync(new FunTranslationRequest
                {Text = pokemonInfo.Description});

            return response.Contents.Translated;
        }
    }
}