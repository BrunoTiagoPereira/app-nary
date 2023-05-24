using AppNary.Core.Exceptions;
using AppNary.Domain.Recipes.Dtos;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Queries.Responses;
using AppNary.Domain.Recipes.Repositories;
using MediatR;

namespace AppNary.Domain.Recipes.Queries.Handlers
{
    public class GetRecipeQueryHandler : IRequestHandler<GetRecipeQueryRequest, GetRecipeQueryResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
        }

        public async Task<GetRecipeQueryResponse> Handle(GetRecipeQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _recipeRepository.GetRecipe(request.RecipeId);

            if (result is null)
            {
                throw new NotFoundException($"A receita {request.RecipeId} não existe");
            }

            return new GetRecipeQueryResponse
            {
                Recipe = new RecipeQueryDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    ImageUrl = result.ImageUrl,
                    LikesCount = result.Likes.Count,
                    Ingredients = result.Ingredients.Select(x => new RecipeIngredientQueryDto
                    {
                        Id = x.IngredientId,
                        SvgIcon = x.Ingredient.SvgIcon,
                        Name = x.Ingredient.Name,
                        UnitOfMeasure = x.Ingredient.UnitOfMeasure,
                    })
                }
            };
        }
    }
}