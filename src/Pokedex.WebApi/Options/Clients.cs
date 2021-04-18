using System.ComponentModel.DataAnnotations;

namespace Pokedex.WebApi.Options
{
    public sealed record Clients
    {
        [Required]
        public string PokemonApiUrl { get; init; }
    }
}