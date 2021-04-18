using FluentAssertions;
using Pokedex.Core.Extensions;
using Xunit;

namespace Pokedex.Core.UnitTests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(
            "It was created by\na scientist after\nyears of horrific.", "It was created by a scientist after years of horrific.")]
        [InlineData(
            "It was created by\fa scientist after\nyears of horrific.", "It was created by a scientist after years of horrific.")]
        public void Should_RemoveLineBreaks_ReturnTheStringWithoutLineBreak(string input, string expected)
        {
            // Arrange
            // Act
            var actual = input.RemoveLineBreaks();

            // Assert
            actual.Should().Be(expected);
        }        
        
        [Theory]
        [InlineData(null, null)]
        [InlineData(
            "It was created by   a scientist after years of   horrific.", "It was created by a scientist after years of horrific.")]
        public void Should_NormalizeSpaces_ReturnTheStringWithoutLineBreak(string input, string expected)
        {
            // Arrange
            // Act
            var actual = input.NormalizeSpaces();

            // Assert
            actual.Should().Be(expected);
        }
    }
}