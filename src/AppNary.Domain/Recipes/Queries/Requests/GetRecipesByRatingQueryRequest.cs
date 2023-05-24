using AppNary.Domain.Recipes.Queries.Responses;
using MediatR;

namespace AppNary.Domain.Recipes.Queries.Requests
{
    public class GetRecipesByRatingQueryRequest : IRequest<GetRecipesByRatingQueryResponse>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Query { get; set; }

        public bool OnlyMine { get; set; }
    }
}