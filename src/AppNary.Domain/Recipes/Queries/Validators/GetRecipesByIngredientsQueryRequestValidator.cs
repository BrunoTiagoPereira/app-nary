using AppNary.Domain.Recipes.Queries.Requests;
using FluentValidation;

namespace AppNary.Domain.Recipes.Queries.Validators
{
    public class GetRecipesByIngredientsQueryRequestValidator : AbstractValidator<GetRecipesByIngredientsQueryRequest>
    {
        public GetRecipesByIngredientsQueryRequestValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0).WithMessage("Page index inválido");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size inválido");
            RuleFor(x => x.IngredientsIds).NotNull().WithMessage("Ingredientes Inválidos");
            When(x => x.IngredientsIds is not null, () =>
            {
                RuleFor(x => x.IngredientsIds).NotEmpty().WithMessage("A pesquisa deve conter pelo menos um ingrediente");
                RuleForEach(x => x.IngredientsIds).NotEmpty().WithMessage("Ingredientes inválidos");
            });
        }
    }
}