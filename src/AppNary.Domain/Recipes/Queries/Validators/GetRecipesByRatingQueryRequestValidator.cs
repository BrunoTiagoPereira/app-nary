using AppNary.Domain.Recipes.Queries.Requests;
using FluentValidation;

namespace AppNary.Domain.Recipes.Queries.Validators
{
    public class GetRecipesByRatingQueryRequestValidator : AbstractValidator<GetRecipesByRatingQueryRequest>
    {
        public GetRecipesByRatingQueryRequestValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0).WithMessage("Page index inválido");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size inválido");
        }
    }
}