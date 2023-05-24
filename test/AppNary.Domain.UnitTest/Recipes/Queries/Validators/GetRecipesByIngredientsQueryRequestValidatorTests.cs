using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Queries.Validators;
using Bogus;

using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Validators
{
    public class GetRecipesByIngredientsQueryRequestValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_validate_when_invalid_page_size(int pageSize)
        {
            // Given
            var validator = new GetRecipesByIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByIngredientsQueryRequest { PageSize = pageSize });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_validate_when_invalid_page_index(int pageIndex)
        {
            // Given
            var validator = new GetRecipesByIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByIngredientsQueryRequest { PageIndex = pageIndex });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PageIndex);
        }

        [Fact]
        public void Should_validate_when_null_ingredients()
        {
            // Given
            var validator = new GetRecipesByIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByIngredientsQueryRequest { IngredientsIds = default(IEnumerable<Guid>) });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.IngredientsIds);
        }

        [Fact]
        public void Should_validate_when_empty_ingredients()
        {
            // Given
            var validator = new GetRecipesByIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByIngredientsQueryRequest { IngredientsIds = Enumerable.Empty<Guid>() });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.IngredientsIds);
        }

        [Fact]
        public void Should_validate_when_any_invalid_ingredient()
        {
            // Given
            var validator = new GetRecipesByIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByIngredientsQueryRequest { IngredientsIds = new List<Guid> { Guid.Empty } });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.IngredientsIds);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new GetRecipesByIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetRecipesByIngredientsQueryRequest { PageIndex = 1, PageSize = 1, IngredientsIds = new List<Guid> { Guid.NewGuid() } });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}