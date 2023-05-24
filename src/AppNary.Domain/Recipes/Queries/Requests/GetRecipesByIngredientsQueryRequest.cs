using AppNary.Domain.Recipes.Queries.Responses;
using MediatR;

namespace AppNary.Domain.Recipes.Queries.Requests
{
    public class GetRecipesByIngredientsQueryRequest : IRequest<GetRecipesByIngredientsQueryResponse>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public IEnumerable<Guid> IngredientsIds { get; set; }
    }
}