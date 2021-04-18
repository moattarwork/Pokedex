using System.Linq;
using AutoFixture;
using FluentAssertions;
using Pokedex.Core.Clients.Poke;
using Pokedex.Core.Extensions;
using Xunit;

namespace Pokedex.Core.UnitTests.Extensions
{
    public class PokemonSpeciesExtensionsTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void Should_RemoveLineBreaks_ReturnTheStringWithoutLineBreak()
        {
            // Arrange
            var species = _fixture.Create<ExtendedPokemonSpecies>();

            // Act
            var actual = species.ToPokemonInfo();

            // Assert
            actual.Name.Should().Be(species.Name);
            actual.IsLegendary.Should().Be(species.IsLegendary);
            actual.Habitat.Should().Be(species.Habitat.Name);
            actual.Description.Should().Be(species.FlavorTextEntries?.FirstOrDefault(m => m.Language.Name == "en")
                ?.FlavorText.RemoveLineBreaks());
        }
    }
}