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
    public class YodaTranslationStrategyTests
    {       
        private readonly Fixture _fixture = new Fixture();
        
        [Theory]
        [InlineData("cave", false, true)]
        [InlineData("rare", true, true)]
        [InlineData("rare", false, false)]
        public void Should_CanTranslate_ReturnsTrue_WhenPokemonIsLegendaryOrHabitatInCave(string habitat, bool legendary, bool expected)
        {
            // Arrange
            var pokemon = new PokemonInfo
            {
                IsLegendary = legendary,
                Habitat = habitat,
            };

            var apiClient = Substitute.For<IFunTranslationClient>();
            var logger = Substitute.For<ILogger<YodaTranslationStrategy>>();

            var sut = new YodaTranslationStrategy(apiClient, logger);

            // Act
            var actual = sut.CanTranslate(pokemon);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("cave", false, true)]
        [InlineData("rare", true, true)]
        [InlineData("rare", false, false)]
        public async Task Should_TranslateAsync_ReturnTranslation_WhenPokemonIsLegendaryOrHabitatInCave(string habitat, bool legendary, bool expected)
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
            apiClient.GetYodaTranslationAsync(Arg.Any<FunTranslationRequest>()).Returns(response);
            var logger = Substitute.For<ILogger<YodaTranslationStrategy>>();

            var sut = new YodaTranslationStrategy(apiClient, logger);

            // Act
            var actual = await sut.TranslateAsync(pokemon);

            // Assert
            actual.Should().Be(expected ? response.Contents.Translated : pokemon.Description);
        }
    }
}