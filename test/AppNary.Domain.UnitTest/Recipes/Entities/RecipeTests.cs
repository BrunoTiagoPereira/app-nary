using AppNary.Core.Exceptions;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Users.Entities;
using AppNary.UnitTest.Abstractions.Fakers;
using Bogus;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Entities
{
    public class RecipeTests
    {
        private readonly Faker _faker;
        private readonly IngredientFaker _ingredientFaker;
        private readonly UserFaker _userFaker;
        private readonly Recipe _recipe;

        public RecipeTests()
        {
            _faker = new Faker();
            _ingredientFaker = new IngredientFaker();
            _recipe = new RecipeFaker().Generate();
            _userFaker = new UserFaker();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_invalid_name(string name)
        {
            FluentActions.Invoking(() => new Recipe(name, _recipe.Description, _recipe.User, _recipe.ImageUrl)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_name_exceeds_max_length()
        {
            FluentActions.Invoking(() => new Recipe(_faker.Lorem.Letter(Recipe.MAX_NAME_LENGTH + 1), _recipe.Description, _recipe.User, _recipe.ImageUrl)).Should().Throw<DomainException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_invalid_description(string description)
        {
            FluentActions.Invoking(() => new Recipe(_recipe.Name, description, _recipe.User, _recipe.ImageUrl)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_description_exceeds_max_length()
        {
            FluentActions.Invoking(() => new Recipe(_recipe.Name, _faker.Lorem.Letter(Recipe.MAX_DESCRIPTION_LENGTH + 1), _recipe.User, _recipe.ImageUrl)).Should().Throw<DomainException>();
        }

        [Fact]
        public void Should_throw_when_invalid_user()
        {
            FluentActions.Invoking(() => new Recipe(_recipe.Name, _recipe.Description, default(User), _recipe.ImageUrl)).Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_throw_when_invalid_image_url(string imageUrl)
        {
            FluentActions.Invoking(() => new Recipe(_recipe.Name, _recipe.Description, _recipe.User, imageUrl)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_img_url_exceeds_max_length()
        {
            FluentActions.Invoking(() => new Recipe(_recipe.Name, _recipe.Description, _recipe.User, _faker.Lorem.Letter(Recipe.MAX_IMAGE_URL_LENGTH + 1))).Should().Throw<DomainException>();
        }

        [Fact]
        public void Should_throw_when_add_null_like()
        {
            FluentActions.Invoking(() => _recipe.AddLike(default(User))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_remove_null_like()
        {
            FluentActions.Invoking(() => _recipe.RemoveLike(default(User))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_add_like_when_does_not_exists()
        {
            // Given
            var user = _userFaker.Generate();

            // When
            _recipe.AddLike(user);

            // Then
            _recipe.Likes.Should().ContainSingle();
            _recipe.Likes.First().User.Should().Be(user);
        }

        [Fact]
        public void Should_add_ingredient()
        {
            // Given
            var ingredient = _ingredientFaker.Generate();

            // When
            _recipe.AddIngredient(ingredient);

            // Then
            _recipe.Ingredients.Should().ContainSingle();
            _recipe.Ingredients.First().Ingredient.Should().Be(ingredient);
        }

        [Fact]
        public void Should_not_add_like_when_already_exists()
        {
            // Given
            var user = _userFaker.Generate();
            _recipe.AddLike(user);

            // When
            _recipe.AddLike(user);

            // Then
            _recipe.Likes.Should().ContainSingle();
            _recipe.Likes.First().User.Should().Be(user);
        }

        [Fact]
        public void Should_get_true_when_image_url_exists()
        {
            // Given / When
            var result = _recipe.HasImage;

            // Then
            result.Should().BeTrue();
        }
        [Fact]
        public void Should_get_false_when_image_url_does_not_exists()
        {
            // Given / When
            var result = new Recipe(_recipe.Name, _recipe.Description, _recipe.User);

            // Then
            result.HasImage.Should().BeFalse();
        }

        [Fact]
        public void Should_remove_image_url()
        {
            // Given / When
            _recipe.RemoveImageUrl();

            // Then
            _recipe.ImageUrl.Should().Be(null);
            _recipe.HasImage.Should().BeFalse();
        }

        [Fact]
        public void Should_clear_ingredients()
        {
            // Given
            _recipe.AddIngredient(_ingredientFaker.Generate());

            // When
            _recipe.ClearIngredients();

            // Then
            _recipe.Ingredients.Should().BeEmpty();
        }

        [Fact]
        public void Should_create()
        {
            // Given
            var name = _faker.Lorem.Word();
            var description = _faker.Lorem.Sentence(2);
            var user = _userFaker.Generate();
            var imageUrl = _faker.Internet.Url();

            // When
            var result = new Recipe(name, description, user, imageUrl);

            // Then
            result.Name.Should().Be(name);
            result.Description.Should().Be(description);
            result.UserId.Should().Be(user.Id);
            result.User.Should().Be(user);
            result.ImageUrl.Should().Be(imageUrl);
        }
    }
}