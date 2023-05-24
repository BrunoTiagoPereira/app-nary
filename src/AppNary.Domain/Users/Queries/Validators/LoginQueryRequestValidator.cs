using FluentValidation;
using ProductsPricing.Domain.Users.Queries.Requests;

namespace AppNary.Domain.Users.Queries.Validators
{
    public class LoginQueryRequestValidator : AbstractValidator<LoginQueryRequest>
    {
        public LoginQueryRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Usuário inválido.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("A senha é obrigatória.");
        }
    }
}