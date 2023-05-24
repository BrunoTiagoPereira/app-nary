using AppNary.Domain.Recipes.Commands.Requests;

using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Validators
{
    public class UploadRecipeImageCommandRequestValidatorTests
    {
        private readonly Mock<IFormFile> _formFile;

        public UploadRecipeImageCommandRequestValidatorTests()
        {
            _formFile = new Mock<IFormFile>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validade_when_invalid_image_file_name(string fileName)
        {
            // Given
            var validator = new UploadRecipeImageCommandRequestValidator();

            _formFile.Setup(x => x.FileName).Returns(fileName);
            _formFile.Setup(x => x.Length).Returns(1);

            // When
            var result = validator.TestValidate(new UploadRecipeImageCommandRequest { Image = _formFile.Object });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Image.FileName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(UploadRecipeImageCommandRequestValidator.MAX_FILE_SIZE_IN_BYTES + 1)]
        public void Should_validade_when_invalid_image_length(int length)
        {
            // Given
            var validator = new UploadRecipeImageCommandRequestValidator();

            _formFile.Setup(x => x.FileName).Returns("file.jpeg");
            _formFile.Setup(x => x.Length).Returns(length);

            // When
            var result = validator.TestValidate(new UploadRecipeImageCommandRequest { Image = _formFile.Object });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Image.Length);
        }

        [Theory]
        [InlineData("file")]
        [InlineData("file.txt")]
        public void Should_validate_when_invalid_image_extension(string fileName)
        {
            // Given
            var validator = new UploadRecipeImageCommandRequestValidator();

            _formFile.Setup(x => x.FileName).Returns(fileName);
            _formFile.Setup(x => x.Length).Returns(1);

            // When
            var result = validator.TestValidate(new UploadRecipeImageCommandRequest { Image = _formFile.Object });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Image.FileName);
        }

        [Fact]
        public void Should_not_validate_when_valid_image_extension()
        {
            // Given
            var validator = new UploadRecipeImageCommandRequestValidator();

            _formFile.Setup(x => x.FileName).Returns("file.png");
            _formFile.Setup(x => x.Length).Returns(1);

            // When
            var result = validator.TestValidate(new UploadRecipeImageCommandRequest { Image = _formFile.Object });

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x.Image.FileName);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new UploadRecipeImageCommandRequestValidator();

            _formFile.Setup(x => x.FileName).Returns("file.png");
            _formFile.Setup(x => x.Length).Returns(1);

            // When
            var result = validator.TestValidate(new UploadRecipeImageCommandRequest { RecipeId = Guid.NewGuid(), Image = _formFile.Object });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}