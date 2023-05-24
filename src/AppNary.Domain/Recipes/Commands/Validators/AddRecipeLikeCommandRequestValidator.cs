using FluentValidation;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class AddRecipeLikeCommandRequestValidator : AbstractValidator<AddRecipeLikeCommandRequest>
    {
        public AddRecipeLikeCommandRequestValidator()
        {
            RuleFor(x => x.RecipeId).NotEmpty().WithMessage("Receita inválida");
        }
    }
}