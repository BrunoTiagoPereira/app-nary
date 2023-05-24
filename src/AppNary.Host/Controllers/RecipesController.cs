using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Host.ApiResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppNary.Host.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    [Authorize]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("{recipeId}")]
        public async Task<ApiResponse> GetRecipe([FromRoute] Guid recipeId)
        {
            var response = await _mediator.Send(new GetRecipeQueryRequest { RecipeId = recipeId });

            return ApiResponse.Success(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<ApiResponse> CreateRecipeAsync([FromBody] CreateRecipeCommandRequest request)
        {
            var response = await _mediator.Send(request);

            return ApiResponse.Success(response);
        }

        [HttpPut]
        [Route("")]
        public async Task<ApiResponse> UpdateRecipeAsync(UpdateRecipeCommandRequest request)
        {
            await _mediator.Send(request);

            return ApiResponse.Success();
        }

        [HttpDelete]
        [Route("{recipeId}")]
        public async Task<ApiResponse> RemoveRecipeAsync([FromRoute] Guid recipeId)
        {
            await _mediator.Send(new RemoveRecipeCommandRequest { RecipeId = recipeId });

            return ApiResponse.Success();
        }

        [HttpPost]
        [Route("upload-image")]
        [RequestSizeLimit(4000000)]
        public async Task<ApiResponse> UploadRecipeImageAsync([FromForm] UploadRecipeImageCommandRequest request)
        {
            var response = await _mediator.Send(request);

            return ApiResponse.Success(response);
        }

        [HttpGet]
        [Route("recipes-by-rating")]
        public async Task<ApiResponse> GetRecipesByRating([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string? query, [FromQuery] bool onlyMine = false)
        {
            var result = await _mediator.Send(new GetRecipesByRatingQueryRequest { PageSize = pageSize, PageIndex = pageIndex,Query = query, OnlyMine = onlyMine });

            return ApiResponse.Success(result);
        }

        [HttpGet]
        [Route("recipes-by-ingredients")]
        public async Task<ApiResponse> GetRecipesByIngredients([FromQuery] IEnumerable<Guid> ingredientsIds, [FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var result = await _mediator.Send(new GetRecipesByIngredientsQueryRequest { PageSize = pageSize, PageIndex = pageIndex, IngredientsIds = ingredientsIds });

            return ApiResponse.Success(result);
        }

        [HttpPost]
        [Route("like")]
        public async Task<ApiResponse> Like(AddRecipeLikeCommandRequest request)
        {
            await _mediator.Send(request);

            return ApiResponse.Success();
        }

        [HttpDelete]
        [Route("remove-like/{recipeId}")]
        public async Task<ApiResponse> RemoveLike(Guid recipeId)
        {
            await _mediator.Send(new RemoveRecipeLikeCommandRequest { RecipeId = recipeId});

            return ApiResponse.Success();
        }

        [HttpGet]
        [Route("ingredients")]
        public async Task<ApiResponse> GetIngredients([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string? query)
        {
            var result = await _mediator.Send(new GetIngredientsQueryRequest { PageSize = pageSize, PageIndex = pageIndex, Query = query });

            return ApiResponse.Success(result);
        }

    }
}