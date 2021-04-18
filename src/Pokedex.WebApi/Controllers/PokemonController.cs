using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Core;
using Pokedex.Core.Domain;
using Pokedex.WebApi.Extensions;

namespace Pokedex.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PokemonController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpGet("{name}")]
        public async Task<ActionResult<PokemonInfo>> Get(string name)
        {
            var result = await _mediator.Send(new PokemonRequest(name));
            return result.ToActionResult();
        } 
        
        [HttpGet("translated/{name}")]
        public async Task<ActionResult<PokemonInfo>> GetTranslated(string name)
        {
            var result = await _mediator.Send(new PokemonTranslatedRequest(name));
            return result.ToActionResult();
        }
    }
}