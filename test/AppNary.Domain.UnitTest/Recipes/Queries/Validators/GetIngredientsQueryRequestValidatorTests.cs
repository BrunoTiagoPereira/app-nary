using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Queries.Validators;

using FluentValidation.TestHelper;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Validators
{
    public class GetIngredientsQueryRequestValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_validate_when_invalid_page_size(int pageSize)
        {
            // Given
            var validator = new GetIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetIngredientsQueryRequest { PageSize = pageSize });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_validate_when_invalid_page_index(int pageIndex)
        {
            // Given
            var validator = new GetIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetIngredientsQueryRequest { PageIndex = pageIndex });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PageIndex);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new GetIngredientsQueryRequestValidator();

            // When
            var result = validator.TestValidate(new GetIngredientsQueryRequest { PageIndex = 1, PageSize = 1 });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}