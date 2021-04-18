using System;
using MediatR;
using Pokedex.Core.Domain;

namespace Pokedex.Core
{
    public sealed class PokemonTranslatedRequest : IRequest<OperationResult<PokemonInfo>>
    {
        public PokemonTranslatedRequest(string pokemonName)
        {
            PokemonName = pokemonName ?? throw new ArgumentNullException(nameof(pokemonName));
        }
        public string PokemonName { get; init; }
    }
}