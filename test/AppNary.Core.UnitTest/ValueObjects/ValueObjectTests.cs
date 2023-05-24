using AppNary.UnitTest.Abstractions.Fakes;
using FluentAssertions;
using System;
using Xunit;

namespace AppNary.Core.UnitTest.ValueObjects
{
    public class ValueObjectTests
    {
        [Fact]
        public void Should_throw_when_invalid_value()
        {
            // Given / When
            var action = () => new ValueObjectFake(null);

            // Then
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_create()
        {
            // Given / When
            var value = "value";
            var enumeration = new ValueObjectFake(value);

            // Then
            enumeration.Value.Should().Be(value);
        }
    }
}