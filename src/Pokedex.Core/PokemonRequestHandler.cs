using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pokedex.Core.Clients;
using Pokedex.Core.Domain;
using Pokedex.Core.Extensions;

namespace Pokedex.Core
{
    public sealed class PokemonRequestHandler : 
        IRequestHandler<PokemonRequest, OperationResult<PokemonInfo>>
        
    {
        private readonly ILogger<PokemonRequestHandler> _logger;
        private readonly IPokeApiClient _pokeClient;

        public PokemonRequestHandler(IPokeApiClient pokeClient, ILogger<PokemonRequestHandler> logger)
        {
            _pokeClient = pokeClient ?? throw new ArgumentNullException(nameof(pokeClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<PokemonInfo>> Handle(PokemonRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var pokemon = (await _pokeClient.GetPokemonSpeciesAsync(request.PokemonName.ToLower())).ToPokemonInfo();

                return OperationResult<PokemonInfo>.Success(pokemon);
            }
            catch (HttpRequestException e)
            {
                var message = $"Error in loading pokemon {request.PokemonName} from API";

                _logger.LogError(message, e);
                return OperationResult<PokemonInfo>.Error(e.StatusCode == HttpStatusCode.NotFound
                        ? OperationErrorReason.ResourceNotFound
                        : OperationErrorReason.GenerricError, message);
            }
        }
    }    
    
    public sealed class PokemonTranslatedRequestHandler : 
        IRequestHandler<PokemonTranslatedRequest, OperationResult<PokemonInfo>>
        
    {
        private readonly ILogger<PokemonTranslatedRequestHandler> _logger;
        private readonly IPokeApiClient _pokeClient;

        public PokemonTranslatedRequestHandler(IPokeApiClient pokeClient, ILogger<PokemonTranslatedRequestHandler> logger)
        {
            _pokeClient = pokeClient ?? throw new ArgumentNullException(nameof(pokeClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<PokemonInfo>> Handle(PokemonTranslatedRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var pokemon = (await _pokeClient.GetPokemonSpeciesAsync(request.PokemonName.ToLower())).ToPokemonInfo();

                var translatedPokemon = pokemon with
                {
                    Description =
                    "Created by a scientist after years of horrific gene splicing and dna engineering experiments, it was."
                };

                return OperationResult<PokemonInfo>.Success(translatedPokemon);
            }
            catch (HttpRequestException e)
            {
                var message = $"Error in loading pokemon {request.PokemonName} from API";

                _logger.LogError(message, e);
                return OperationResult<PokemonInfo>.Error(e.StatusCode == HttpStatusCode.NotFound
                        ? OperationErrorReason.ResourceNotFound
                        : OperationErrorReason.GenerricError, message);
            }
        }
    }
}