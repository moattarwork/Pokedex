using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pokedex.Core.Clients.Poke;
using Pokedex.Core.Domain;
using Pokedex.Core.Extensions;
using Pokedex.Core.Translations;

namespace Pokedex.Core
{
    public sealed class
        PokemonTranslatedRequestHandler : IRequestHandler<PokemonTranslatedRequest, OperationResult<PokemonInfo>>
    {
        private readonly ILogger<PokemonTranslatedRequestHandler> _logger;
        private readonly IPokeApiClient _pokeClient;
        private readonly IEnumerable<ITranslationStrategy> _translationStrategies;

        public PokemonTranslatedRequestHandler(IPokeApiClient pokeClient,
            IEnumerable<ITranslationStrategy> translationStrategies, 
            ILogger<PokemonTranslatedRequestHandler> logger)
        {
            _pokeClient = pokeClient ?? throw new ArgumentNullException(nameof(pokeClient));
            _translationStrategies =
                translationStrategies ?? throw new ArgumentNullException(nameof(translationStrategies));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<PokemonInfo>> Handle(PokemonTranslatedRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            _logger.LogInformation($"Process translate request for Pokemon {request.PokemonName}");

            try
            {
                var pokemon = (await _pokeClient.GetPokemonSpeciesAsync(request.PokemonName.ToLower())).ToPokemonInfo();
                
                _logger.LogInformation($"Pokemon {request.PokemonName} information has retrived successfully");
                
                var translatedPokemon = pokemon with
                {
                    Description = await TranslateAsync(pokemon)
                };

                _logger.LogInformation($"Pokemon {request.PokemonName} description is translated successfully");
                
                return OperationResult<PokemonInfo>.Success(translatedPokemon);
            }
            catch (HttpRequestException e)
            {
                var message = $"Error in loading pokemon {request.PokemonName} from API";

                _logger.LogError(message, e);
                return OperationResult<PokemonInfo>.Error(e.StatusCode == HttpStatusCode.NotFound
                    ? OperationErrorReason.ResourceNotFound
                    : OperationErrorReason.GenericError, message);
            }
        }

        private async Task<string> TranslateAsync(PokemonInfo pokemon)
        {
            var translator = _translationStrategies.FirstOrDefault(s => s.CanTranslate(pokemon));
            if (translator == null)
            {
                _logger.LogWarning($"No translation strategy found for {pokemon.Name}.");
                return pokemon.Description;
            }

            try
            {
                return await translator.TranslateAsync(pokemon);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in traslation {pokemon.Name} description.", e);
                return pokemon.Description;
            }
        }
    }
}