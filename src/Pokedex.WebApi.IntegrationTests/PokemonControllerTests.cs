using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Pokedex.Core.Domain;
using Pokedex.WebApi.IntegrationTests.Extensions;
using Xunit;

namespace Pokedex.WebApi.IntegrationTests
{
    public class PokemonControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public PokemonControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Theory]
        [InlineData("mewtwo", "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments", "rare", true)]
        public async Task Should_Get_ReturnBasicPokemonInformationCorrectly_WhenTheCorrectNameForPokemonIsProvided(
            string name, string description, string habitat, bool isLegendary)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"pokemon/{name}");

            // Assert
            var actual = await response.ValidateAndReadContentAsync<Pokemon>();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(description);
            actual.Habitat.Should().Be(habitat);
            actual.IsLegendary.Should().Be(isLegendary);
        }
    }
}