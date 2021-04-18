using System.ComponentModel.DataAnnotations;

namespace Pokedex.WebApi
{
    public sealed record Clients
    {
        [Required]
        public string PokemonApiUrl { get; init; }
    }
}