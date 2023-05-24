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
    public class CreateRecipeCommandRequestValidatorTests
    {
        private readonly Faker _faker;

        public CreateRecipeCommandRequestValidatorTests()
        {
            _faker = new Faker();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_when_invalid_name(string name)
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Name = name });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_validade_when_name_exceeds_max_characters()
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Name = _faker.Lorem.Letter(Recipe.MAX_NAME_LENGTH + 1) });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_when_invalid_description(string description)
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Description = description });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_validade_when_description_exceeds_max_characters()
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Description = _faker.Lorem.Letter(Recipe.MAX_DESCRIPTION_LENGTH + 1) });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_validade_when_null_ingredients()
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Ingredients = default(List<RecipeIngredientQueryRequestDto>) });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Ingredients);
        }

        [Fact]
        public void Should_validade_when_empty_ingredients()
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Ingredients = new List<RecipeIngredientQueryRequestDto>() });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Ingredients);
        }

        [Fact]
        public void Should_validade_when_any_ingredient_id_is_invalid()
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();
            var ingredient = new RecipeIngredientQueryRequestDto { IngredientId = Guid.Empty };

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Ingredients = new List<RecipeIngredientQueryRequestDto> { ingredient } });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Ingredients);
        }


        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new CreateRecipeCommandRequestValidator();
            var recipe = new RecipeFaker().Generate();
            var ingredient = new RecipeIngredientQueryRequestDto { IngredientId = Guid.NewGuid() };

            // When
            var result = validator.TestValidate(new CreateRecipeCommandRequest { Name = recipe.Name, Description = recipe.Description, Ingredients = new List<RecipeIngredientQueryRequestDto> { ingredient } });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}