using AppNary.Domain.Users.Entities;
using AppNary.UnitTest.Abstractions.Fakers;
using Bogus;
using FluentAssertions;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Entities
{
    public class UserTests
    {
        private readonly Faker _faker;
        private readonly User _user;

        public UserTests()
        {
            _faker = new Faker();
            _user = new UserFaker().Generate();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_invalid_user_name(string userName)
        {
            FluentActions.Invoking(() => new User(userName, _faker.Internet.Password())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_create()
        {
            // Given / When
            var user = new User(_user.UserName, _user.Password.Hash);

            // Then
            user.UserName.Should().Be(_user.UserName);
            user.Password.Should().NotBeNull();
            user.Password.Hash.Should().NotBeNullOrWhiteSpace();
        }
    }
}