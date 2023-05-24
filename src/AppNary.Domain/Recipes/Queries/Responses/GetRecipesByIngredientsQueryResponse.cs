using AppNary.Core.Application.Pagination;
using AppNary.Domain.Recipes.Dtos;

namespace AppNary.Domain.Recipes.Queries.Responses
{
    public class GetRecipesByIngredientsQueryResponse
    {
        public PagedResult<RecipeItemQueryDto> Result { get; set; }
    }

}