using AppNary.Domain.Recipes.Queries.Responses;
using MediatR;

namespace AppNary.Domain.Recipes.Queries.Requests
{
    public class GetIngredientsQueryRequest : IRequest<GetIngredientsQueryResponse>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Query { get; set; }
    }
}