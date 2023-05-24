using FluentValidation;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class RemoveRecipeLikeCommandRequestValidator : AbstractValidator<RemoveRecipeLikeCommandRequest>
    {
        public RemoveRecipeLikeCommandRequestValidator()
        {
            RuleFor(x => x.RecipeId).NotEmpty().WithMessage("Receita inválida");
        }
    }
}