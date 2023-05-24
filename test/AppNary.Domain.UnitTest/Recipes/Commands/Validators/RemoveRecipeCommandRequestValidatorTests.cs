using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Commands.Validators;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Commands.Validators
{
    public class RemoveRecipeCommandRequestValidatorTests
    {
        [Fact]
        public void Should_validate_when_invalid_id()
        {
            // Given
            var validator = new RemoveRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new RemoveRecipeCommandRequest { RecipeId = Guid.Empty });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.RecipeId);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new RemoveRecipeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new RemoveRecipeCommandRequest { RecipeId = Guid.NewGuid() });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}