using AppNary.Domain.Users.Commands.Requests;
using FluentValidation;

namespace AppNary.Domain.Users.Commands.Validators
{
    public class CreateUserCommandRequestValidator : AbstractValidator<CreateUserCommandRequest>
    {
        public CreateUserCommandRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("O nome de usuário não é válido.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("A senha é obrigatória.");

            RuleFor(x => x.PasswordConfirmation)
                .Matches(x => x.Password).WithMessage("As senhas são diferentes")
                .When(x => !string.IsNullOrWhiteSpace(x.Password))
                ;
        }
    }
}