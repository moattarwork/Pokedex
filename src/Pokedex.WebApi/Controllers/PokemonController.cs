using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Core.Domain;

namespace Pokedex.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        [HttpGet("{name}")]
        public ActionResult<Pokemon> Get(string name)
        {
            var pokemon = new Pokemon
            {
                Name = name,
                Description =
                    "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments",
                Habitat = "rare",
                IsLegendary = true
            };
            return Ok(pokemon);
        }
    }
}