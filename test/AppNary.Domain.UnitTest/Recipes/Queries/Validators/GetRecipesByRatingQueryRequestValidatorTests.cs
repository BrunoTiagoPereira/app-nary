using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Queries.Validators;

using FluentValidation.TestHelper;
using System.Collections.Generic;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Validators
{
    public class GetRecipesByRatingQueryRequestValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_validate_when_invalid_page_size(int pageSize)
        {
            // Given
            var validator = new GetRecipesByRatingQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByRatingQueryRequest { PageSize = pageSize });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_validate_when_invalid_page_index(int pageIndex)
        {
            // Given
            var validator = new GetRecipesByRatingQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByRatingQueryRequest { PageIndex = pageIndex });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PageIndex);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new GetRecipesByRatingQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByRatingQueryRequest { PageIndex = 1, PageSize = 1 });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}