using AppNary.Domain.Recipes.Commands.Requests;
using FluentValidation;

namespace AppNary.Domain.Recipes.Commands.Validators
{
    public class RemoveRecipeCommandRequestValidator : AbstractValidator<RemoveRecipeCommandRequest>
    {
        public RemoveRecipeCommandRequestValidator()
        {
            RuleFor(x => x.RecipeId).NotEmpty().WithMessage("Receita inválida");
        }
    }
}