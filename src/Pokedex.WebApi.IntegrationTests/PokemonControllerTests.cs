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
        [InlineData("mewtwo", "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.", "rare", true)]
        [InlineData("ditto", "It can freely recombine its own cellular structure to transform into other life-forms.", "urban", false)]
        [InlineData("zubat", "Forms colonies in perpetually dark places. Uses ultrasonic waves to identify and approach targets.", "cave", false)]
        public async Task Should_Get_ReturnBasicPokemonInformationCorrectly_WhenTheCorrectNameForPokemonIsProvided(
            string name, string description, string habitat, bool isLegendary)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"pokemon/{name}");

            // Assert
            var actual = await response.ValidateAndReadContentAsync<PokemonInfo>();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(description);
            actual.Habitat.Should().Be(habitat);
            actual.IsLegendary.Should().Be(isLegendary);
        }
        
                
        [Theory]
        [InlineData("mewtwo", "rare", true, "Created by a scientist after years of horrific gene splicing and dna engineering experiments, it was.")]
        [InlineData("ditto", "urban", false, "'t can freely recombine its own cellular structure to transform into other life-forms.")]
        [InlineData("zubat", "cave", false, "Forms colonies in perpetually dark places.Ultrasonic waves to identify and approach targets, uses.")]
        public async Task Should_GetTranslated_ReturnPokemonWithTranslatedDescription_BasedOnTheCorrectHabitat(
            string name, string habitat, bool isLegendary, string expectedDescription)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"pokemon/translated/{name}");

            // Assert
            var actual = await response.ValidateAndReadContentAsync<PokemonInfo>();
            actual.Name.Should().Be(name);
            actual.Description.Should().Be(expectedDescription);
            actual.Habitat.Should().Be(habitat);
            actual.IsLegendary.Should().Be(isLegendary);
        }
    }
}