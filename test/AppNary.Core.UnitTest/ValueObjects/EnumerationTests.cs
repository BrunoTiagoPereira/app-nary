using AppNary.UnitTest.Abstractions.Fakes;
using FluentAssertions;
using System;
using Xunit;

namespace AppNary.Core.UnitTest.ValueObjects
{
    public class EnumerationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_throw_when_invalid_name(string name)
        {
            // Given / When
            var action = () => new EnumerationFake<int>(name, 1);

            // Then
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_create()
        {
            // Given / When
            var name = "name";
            var enumeration = new EnumerationFake<int>(name, 1);

            // Then
            enumeration.Name.Should().Be(name);
        }
    }
}