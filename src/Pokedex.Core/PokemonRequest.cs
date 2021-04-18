using System;
using MediatR;
using Pokedex.Core.Domain;

namespace Pokedex.Core
{
    public sealed class PokemonRequest : IRequest<OperationResult<PokemonInfo>>
    {
        public PokemonRequest(string pokemonName)
        {
            PokemonName = pokemonName ?? throw new ArgumentNullException(nameof(pokemonName));
        }
        public string PokemonName { get; init; }
    }
}