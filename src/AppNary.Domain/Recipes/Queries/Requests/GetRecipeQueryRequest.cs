using AppNary.Domain.Recipes.Queries.Responses;
using MediatR;

namespace AppNary.Domain.Recipes.Queries.Requests
{
    public class GetRecipeQueryRequest : IRequest<GetRecipeQueryResponse>
    {
        public Guid RecipeId { get; set; }
    }
}