using AppNary.Core.Application.Pagination;
using AppNary.Domain.Recipes.Dtos;

namespace AppNary.Domain.Recipes.Queries.Responses
{
    public class GetIngredientsQueryResponse
    {
        public PagedResult<IngredientDto> Result { get; set; }
    }
}