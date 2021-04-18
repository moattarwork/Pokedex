using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Pokedex.Core.Clients.Poke;
using Pokedex.Core.Domain;
using Pokedex.Core.Translations;
using Xunit;

namespace Pokedex.Core.UnitTests
{
    public class PokemonTranslatedRequestHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        
        [Fact]
        public async Task Should_Handle_ReturnTheTranslatedPokemonInfo_WhenTheRequestedNameIsCorrect()
        {
            // Arrange
            var species = _fixture.Create<ExtendedPokemonSpecies>();
            species.Name = "name";
            species.IsLegendary = true;
            species.Habitat.Name = "rare";
            species.FlavorTextEntries[0].Language.Name = "en";
            species.FlavorTextEntries[0].FlavorText = "Text Line1\nText Line 2";

            var apiClient = Substitute.For<IPokeApiClient>();
            apiClient.GetPokemonSpeciesAsync(Arg.Any<string>()).Returns(Task.FromResult(species));

            var translationStrategy = Substitute.For<ITranslationStrategy>();
            translationStrategy.CanTranslate(Arg.Any<PokemonInfo>()).Returns(true);
            translationStrategy.TranslateAsync(Arg.Any<PokemonInfo>()).Returns(Task.FromResult("translated description"));
            
            var logger = Substitute.For<ILogger<PokemonTranslatedRequestHandler>>();

            var sut = new PokemonTranslatedRequestHandler(apiClient, new[] {translationStrategy}, logger);

            // Act
            var actual = await sut.Handle(new PokemonTranslatedRequest("name"), CancellationToken.None);

            // Assert
            actual.Succeed.Should().BeTrue();
            actual.Result.Should().NotBeNull();
            actual.Result.Name.Should().Be("name");
            actual.Result.Habitat.Should().Be("rare");
            actual.Result.Description.Should().Be("translated description");
            actual.Result.IsLegendary.Should().BeTrue();
        }            
        
        [Fact]
        public async Task Should_Handle_ReturnThePokemonInfoWithNoTranslation_WhenTheRequestedNameIsCorrectButTranslatorThrowError()
        {
            // Arrange
            var species = _fixture.Create<ExtendedPokemonSpecies>();
            species.Name = "name";
            species.IsLegendary = true;
            species.Habitat.Name = "rare";
            species.FlavorTextEntries[0].Language.Name = "en";
            species.FlavorTextEntries[0].FlavorText = "Text Line1\nText Line 2";

            var apiClient = Substitute.For<IPokeApiClient>();
            apiClient.GetPokemonSpeciesAsync(Arg.Any<string>()).Returns(Task.FromResult(species));

            var translationStrategy = Substitute.For<ITranslationStrategy>();
            translationStrategy.CanTranslate(Arg.Any<PokemonInfo>()).Returns(true);
            translationStrategy.TranslateAsync(Arg.Any<PokemonInfo>()).Throws(new Exception("Error from the translator"));
            
            var logger = Substitute.For<ILogger<PokemonTranslatedRequestHandler>>();

            var sut = new PokemonTranslatedRequestHandler(apiClient, new[] {translationStrategy}, logger);

            // Act
            var actual = await sut.Handle(new PokemonTranslatedRequest("name"), CancellationToken.None);

            // Assert
            actual.Succeed.Should().BeTrue();
            actual.Result.Should().NotBeNull();
            actual.Result.Name.Should().Be("name");
            actual.Result.Habitat.Should().Be("rare");
            actual.Result.Description.Should().Be("Text Line1 Text Line 2");
            actual.Result.IsLegendary.Should().BeTrue();
        }        
        
        [Fact]
        public async Task Should_Handle_ReturnNotFoundError_WhenTheRequestedNameIsNotFound()
        {
            // Arrange
            var apiClient = Substitute.For<IPokeApiClient>();
            apiClient.GetPokemonSpeciesAsync(Arg.Any<string>()).Throws(new HttpRequestException("error", null, HttpStatusCode.NotFound));
            
            var translationStrategy = Substitute.For<ITranslationStrategy>();
            translationStrategy.CanTranslate(Arg.Any<PokemonInfo>()).Returns(true);
            
            var logger = Substitute.For<ILogger<PokemonTranslatedRequestHandler>>();
            
            var sut = new PokemonTranslatedRequestHandler(apiClient, new[] {translationStrategy}, logger);

            // Act
            var actual = await sut.Handle(new PokemonTranslatedRequest("name"), CancellationToken.None);

            // Assert
            actual.Failed.Should().BeTrue();
            actual.Result.Should().BeNull();
            actual.ErrorReason.Should().Be(OperationErrorReason.ResourceNotFound);
            actual.ErrorMessage.Should().Be($"Error in loading pokemon name from API");
        }          
        
        [Fact]
        public async Task Should_Handle_ReturnGenericError_WhenClientReturnsOtherErrors()
        {
            // Arrange
            var apiClient = Substitute.For<IPokeApiClient>();
            apiClient.GetPokemonSpeciesAsync(Arg.Any<string>()).Throws(new HttpRequestException("error", null, HttpStatusCode.Forbidden));
            
            var translationStrategy = Substitute.For<ITranslationStrategy>();
            translationStrategy.CanTranslate(Arg.Any<PokemonInfo>()).Returns(true);
            
            var logger = Substitute.For<ILogger<PokemonTranslatedRequestHandler>>();
            
            var sut = new PokemonTranslatedRequestHandler(apiClient, new[] {translationStrategy}, logger);

            // Act
            var actual = await sut.Handle(new PokemonTranslatedRequest("name"), CancellationToken.None);

            // Assert
            actual.Failed.Should().BeTrue();
            actual.Result.Should().BeNull();
            actual.ErrorReason.Should().Be(OperationErrorReason.GenerricError);
            actual.ErrorMessage.Should().Be($"Error in loading pokemon name from API");
        }  
    }
}