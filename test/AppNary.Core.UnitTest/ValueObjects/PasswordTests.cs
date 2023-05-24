using AppNary.Core.ValueObjects;
using Bogus;
using FluentAssertions;
using System;
using Xunit;

namespace AppNary.Core.UnitTest.ValueObjects
{
    public class PasswordTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_invalid_password(string password)
        {
            FluentActions.Invoking(() => new Password(password)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_create()
        {
            // Given
            var password = new Faker().Internet.Password();

            // When
            var result = new Password(password);

            // Then
            result.Hash.Should().NotBeNullOrWhiteSpace();
            result.Hash.Should().NotBe(password);
        }
    }
}