using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pokedex.Core.Clients;
using Pokedex.Core.Clients.Poke;
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

            _logger.LogInformation($"Process translate request for Pokemon {request.PokemonName}");
            
            try
            {
                var pokemon = (await _pokeClient.GetPokemonSpeciesAsync(request.PokemonName.ToLower())).ToPokemonInfo();
                
                _logger.LogInformation($"Pokemon {request.PokemonName} information has retrived successfully");

                return OperationResult<PokemonInfo>.Success(pokemon);
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
    }
}