using AppNary.Domain.Recipes.Entities;
using AppNary.UnitTest.Abstractions.Fakers;
using FluentAssertions;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Entities
{
    public class RecipeIngredientTests
    {
        private readonly IngredientFaker _ingredientFaker;
        private readonly RecipeFaker _recipeFaker;

        public RecipeIngredientTests()
        {
            _ingredientFaker = new IngredientFaker();
            _recipeFaker = new RecipeFaker();
        }

        [Fact]
        public void Should_throw_when_invalid_recipe()
        {
            FluentActions.Invoking(() => new RecipeIngredient(default(Recipe), _ingredientFaker.Generate())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_invalid_ingredient()
        {
            FluentActions.Invoking(() => new RecipeIngredient(_recipeFaker.Generate(), default(Ingredient))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_create()
        {
            // Given
            var recipe = _recipeFaker.Generate();
            var ingredient = _ingredientFaker.Generate();

            // When
            var result = new RecipeIngredient(recipe, ingredient);

            // Then
            result.RecipeId.Should().Be(recipe.Id);
            result.Recipe.Should().Be(recipe);
            result.IngredientId.Should().Be(ingredient.Id);
            result.Ingredient.Should().Be(ingredient);
        }
    }
}