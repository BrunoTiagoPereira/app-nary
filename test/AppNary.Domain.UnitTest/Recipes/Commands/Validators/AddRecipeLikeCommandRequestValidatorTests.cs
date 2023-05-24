using AppNary.Domain.Recipes.Commands.Requests;

using FluentValidation.TestHelper;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Validators
{
    public class AddRecipeLikeCommandRequestValidatorTests
    {
        [Fact]
        public void Should_validate_when_invalid_recipe_id()
        {
            // Given
            var validator = new AddRecipeLikeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new AddRecipeLikeCommandRequest { RecipeId = Guid.Empty });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.RecipeId);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new AddRecipeLikeCommandRequestValidator();

            // When
            var result = validator.TestValidate(new AddRecipeLikeCommandRequest { RecipeId = Guid.NewGuid() });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}