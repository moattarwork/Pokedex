using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Pokedex.Core.Clients.FunTranslation;
using Pokedex.Core.Domain;
using Pokedex.Core.Translations;
using Xunit;

namespace Pokedex.Core.UnitTests.Translations
{
    public class ShakespeareTranslationStrategyTests 
    {
        private readonly Fixture _fixture = new Fixture();
        
        [Theory]
        [InlineData("cave", false, false)]
        [InlineData("rare", true, false)]
        [InlineData("rare", false, true)]
        public void Should_CanTranslate_ReturnsTrue_WhenPokemonIsNotLegendaryOrNotHabitatInCave(string habitat, bool legendary, bool expected)
        {
            // Arrange
            var pokemon = new PokemonInfo
            {
                IsLegendary = legendary,
                Habitat = habitat,
            };

            var apiClient = Substitute.For<IFunTranslationClient>();
            var logger = Substitute.For<ILogger<ShakespeareTranslationStrategy>>();

            var sut = new ShakespeareTranslationStrategy(apiClient, logger);

            // Act
            var actual = sut.CanTranslate(pokemon);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("cave", false, false)]
        [InlineData("rare", true, false)]
        [InlineData("rare", false, true)]
        public async Task Should_TranslateAsync_ReturnPokemon_WhenPokemonIsNotLegendaryOrNotHabitatInCave(string habitat, bool legendary, bool expected)
        {
            // Arrange
            var pokemon = new PokemonInfo
            {
                IsLegendary = legendary,
                Habitat = habitat,
                Description = "Pokemon Description"
            };

            var response = _fixture.Create<FunTranslationResponse>();

            var apiClient = Substitute.For<IFunTranslationClient>();
            apiClient.GetShakespeareTranslationAsync(Arg.Any<FunTranslationRequest>()).Returns(response);
            var logger = Substitute.For<ILogger<ShakespeareTranslationStrategy>>();

            var sut = new ShakespeareTranslationStrategy(apiClient, logger);

            // Act
            var actual = await sut.TranslateAsync(pokemon);

            // Assert
            actual.Should().Be(expected ? response.Contents.Translated : pokemon.Description);
        }
    }
}