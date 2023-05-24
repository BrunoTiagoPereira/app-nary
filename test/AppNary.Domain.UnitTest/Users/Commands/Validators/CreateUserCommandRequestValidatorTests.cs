using AppNary.Domain.Users.Commands.Requests;
using AppNary.Domain.Users.Commands.Validators;
using Bogus;

using FluentValidation.TestHelper;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Validators
{
    public class CreateUserCommandRequestValidatorTests
    {
        private readonly Faker _faker = new();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_when_invalid_username(string userName)
        {
            // Given
            var validator = new CreateUserCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateUserCommandRequest { UserName = userName });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.UserName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_validate_when_invalid_password(string password)
        {
            // Given
            var validator = new CreateUserCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateUserCommandRequest { Password = password });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_validate_when_passwords_dont_match()
        {
            // Given
            var validator = new CreateUserCommandRequestValidator();

            // When
            var result = validator.TestValidate(new CreateUserCommandRequest { Password = _faker.Internet.Password(), PasswordConfirmation = _faker.Internet.Password() });

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PasswordConfirmation);
        }

        [Fact]
        public void Should_not_have_any_validation_errors()
        {
            // Given
            var validator = new CreateUserCommandRequestValidator();
            var password = _faker.Internet.Password(15);

            // When
            var result = validator.TestValidate(new CreateUserCommandRequest { UserName = _faker.Internet.Email(), Password = password, PasswordConfirmation = password });

            // Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}