using AppNary.Core.Application.Pagination;
using AppNary.Domain.Recipes.Dtos;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Queries.Responses;
using AppNary.Domain.Recipes.Repositories;
using MediatR;

namespace AppNary.Domain.Recipes.Queries.Handlers
{
    public class GetIngredientsQueryHandler : IRequestHandler<GetIngredientsQueryRequest, GetIngredientsQueryResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetIngredientsQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
        }

        public async Task<GetIngredientsQueryResponse> Handle(GetIngredientsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _recipeRepository.GetIngredientsAsync(request.PageSize, request.PageIndex, request.Query);

            return new GetIngredientsQueryResponse
            {
                Result = new PagedResult<IngredientDto>
                {
                    Items = result.Items.Select(x => new IngredientDto { Id = x.Id, Name = x.Name, UnitOfMeasure = x.UnitOfMeasure, SvgIcon = x.SvgIcon }),
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize,
                    Query = result.Query,
                    TotalResults = result.TotalResults
                }
            };
        }
    }
}