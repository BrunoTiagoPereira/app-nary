using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Users.Entities;
using AppNary.UnitTest.Abstractions.Fakers;
using Bogus;
using FluentAssertions;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Entities
{
    public class LikeTests
    {
        private readonly UserFaker _userFaker;
        private readonly RecipeFaker _recipeFaker;

        public LikeTests()
        {
            _userFaker = new UserFaker();
            _recipeFaker = new RecipeFaker();
        }

        [Fact]
        public void Should_throw_when_invalid_user()
        {
            FluentActions.Invoking(() => new Like(default(User), _recipeFaker.Generate())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_invalid_recipe()
        {
            FluentActions.Invoking(() => new Like(_userFaker.Generate(), default(Recipe))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_create()
        {
            // Given
            var user = _userFaker.Generate();
            var recipe = _recipeFaker.Generate();

            // When
            var result = new Like(user, recipe);

            // Then
            result.UserId.Should().Be(user.Id);
            result.User.Should().Be(user);
            result.RecipeId.Should().Be(recipe.Id);
            result.Recipe.Should().Be(recipe);
        }
    }
}