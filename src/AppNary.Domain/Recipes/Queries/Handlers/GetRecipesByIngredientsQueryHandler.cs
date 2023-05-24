using AppNary.Core.Application.Pagination;
using AppNary.Domain.Recipes.Dtos;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Queries.Responses;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Users.Managers;
using MediatR;

namespace AppNary.Domain.Recipes.Queries.Handlers
{
    public class GetRecipesByIngredientsQueryHandler : IRequestHandler<GetRecipesByIngredientsQueryRequest, GetRecipesByIngredientsQueryResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUserAccessorManager _userAccessorManager;

        public GetRecipesByIngredientsQueryHandler(IRecipeRepository recipeRepository, IUserAccessorManager userAccessorManager)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
            _userAccessorManager = userAccessorManager ?? throw new ArgumentNullException(nameof(userAccessorManager));
        }

        public async Task<GetRecipesByIngredientsQueryResponse> Handle(GetRecipesByIngredientsQueryRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = _userAccessorManager.GetCurrentUserId();
            var result = await _recipeRepository.GetRecipesByIngredientsIdsAsync(request.IngredientsIds, request.PageSize, request.PageIndex);

            return new GetRecipesByIngredientsQueryResponse
            {
                Result = new PagedResult<RecipeItemQueryDto>
                {
                    Items = result.Items.Select(x => new RecipeItemQueryDto { Id = x.Id, Name = x.Name, Description = x.Description, ImageUrl = x.ImageUrl, LikesCount = x.Likes.Count(), UserHasLiked = x.Likes.Any(x => x.UserId == currentUserId) }),
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize,
                    TotalResults = result.TotalResults
                }
            };
        }
    }
}