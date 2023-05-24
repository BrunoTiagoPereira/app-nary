using AppNary.Domain.Recipes.Queries.Requests;
using FluentValidation;

namespace AppNary.Domain.Recipes.Queries.Validators
{
    public class GetRecipeQueryRequestValidator : AbstractValidator<GetRecipeQueryRequest>
    {
        public GetRecipeQueryRequestValidator()
        {
            RuleFor(x => x.RecipeId).NotEmpty().WithMessage("Receita inválida");
        }
    }
}