using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Dtos;
using AppNary.Domain.Recipes.Entities;
using AppNary.UnitTest.Abstractions.Fakers;
using Bogus;

using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Validators
{
    public class UpdateRecipeCommandRequestValidatorTests
    {
        private readonly Faker _faker;

        public UpdateRecipeCommandRequestValidatorTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void Should_validade_when_empty_recipe_id()
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { RecipeId = Guid.Empty });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.RecipeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_when_invalid_name(string name)
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { Name = name });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_validade_when_name_exceeds_max_characters()
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { Name = _faker.Lorem.Letter(Recipe.MAX_NAME_LENGTH + 1) });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_when_invalid_description(string description)
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { Description = description });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_validade_when_description_exceeds_max_characters()
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { Description = _faker.Lorem.Letter(Recipe.MAX_DESCRIPTION_LENGTH + 1) });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_validade_when_null_ingredients()
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { Ingredients = default(List<RecipeIngredientQueryRequestDto>) });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Ingredients);
        }

        [Fact]
        public void Should_validade_when_empty_ingredients()
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { Ingredients = new List<RecipeIngredientQueryRequestDto>() });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Ingredients);
        }

        [Fact]
        public void Should_validade_when_any_ingredient_id_is_invalid()
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();
            var ingredient = new RecipeIngredientQueryRequestDto { IngredientId = Guid.Empty };

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { Ingredients = new List<RecipeIngredientQueryRequestDto> { ingredient } });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Ingredients);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new UpdateRecipeCommandRequestValidator();
            var recipe = new RecipeFaker().Generate();
            var ingredient = new RecipeIngredientQueryRequestDto { IngredientId = Guid.NewGuid() };

            // When
            var result = validator.TestValidate(new UpdateRecipeCommandRequest { RecipeId = recipe.Id, Name = recipe.Name, Description = recipe.Description, Ingredients = new List<RecipeIngredientQueryRequestDto> { ingredient } });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}