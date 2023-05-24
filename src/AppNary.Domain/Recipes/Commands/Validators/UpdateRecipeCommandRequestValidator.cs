using AppNary.Domain.Recipes.Entities;
using FluentValidation;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class UpdateRecipeCommandRequestValidator : AbstractValidator<UpdateRecipeCommandRequest>
    {
        public UpdateRecipeCommandRequestValidator()
        {
            RuleFor(x => x.RecipeId).NotEmpty().WithMessage("Receita inválida");

            RuleFor(x => x.Name).NotEmpty().WithMessage("O nome deve ser válido.");
            RuleFor(x => x.Name).MaximumLength(Recipe.MAX_NAME_LENGTH).WithMessage("O nome deve ter no máximo {MaxLength} caracteres.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("A descrição deve ser válida.");
            RuleFor(x => x.Description).MaximumLength(Recipe.MAX_DESCRIPTION_LENGTH).WithMessage("A descrição deve ter no máximo {MaxLength} caracteres.");

            RuleFor(x => x.Ingredients).NotNull().WithMessage("A receita deve conter no mínimo 1 ingrediente");

            When(x => x.Ingredients != null, () =>
            {
                RuleFor(x => x.Ingredients).NotEmpty().WithMessage("A receita deve conter no mínimo 1 ingrediente");
                RuleForEach(x => x.Ingredients).Must(x => x.IngredientId != default).WithMessage("Ingredientes inválidos");
            });
        }
    }
}