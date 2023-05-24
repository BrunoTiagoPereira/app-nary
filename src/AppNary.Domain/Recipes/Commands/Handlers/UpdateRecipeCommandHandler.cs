using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Commands.Responses;
using AppNary.Domain.Recipes.Dtos;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Users.Managers;
using MediatR;

namespace AppNary.Domain.Users.Commands.Handlers
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommandRequest, UpdateRecipeCommandResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IUserAccessorManager _userAccessorManager;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository, IUnitOfWork uow, IUserAccessorManager userAccessorManager)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userAccessorManager = userAccessorManager ?? throw new ArgumentNullException(nameof(userAccessorManager));
        }

        public async Task<UpdateRecipeCommandResponse> Handle(UpdateRecipeCommandRequest request, CancellationToken cancellationToken)
        {
            var recipe = await GetRecipe(request.RecipeId);

            _userAccessorManager.ThrowIfUserDontHasAccess(recipe);

            var requestIngredientsIds = GetRequestIngredientsIds(request);

            await ThrowIfAnyIngredientDoesNotExists(requestIngredientsIds);

            recipe.UpdateName(request.Name);
            recipe.UpdateDescription(request.Description);

            var ingredients = await _recipeRepository.GetIngredientsFromIdsAsync(requestIngredientsIds);

            recipe.ClearIngredients();
            AddIngredientsToRecipe(recipe, ingredients, request.Ingredients);

            _recipeRepository.Update(recipe);
            await _uow.CommitAsync();

            return new UpdateRecipeCommandResponse();
        }

        private async Task<Recipe> GetRecipe(Guid recipeId)
        {
            var recipe = await _recipeRepository.GetRecipe(recipeId);

            if(recipe is null)
            {
                throw new DomainException("A receita não existe");
            }

            return recipe;
        }

        private static IEnumerable<Guid> GetRequestIngredientsIds(UpdateRecipeCommandRequest request)
        {
            return request.Ingredients.Select(x => x.IngredientId);
        }

        private async Task ThrowIfAnyIngredientDoesNotExists(IEnumerable<Guid> requestIngredientsIds)
        {
            var anyIngredientDoesNotExists = await _recipeRepository.AnyIngredientDoesNotExistsAsync(requestIngredientsIds);

            if (anyIngredientDoesNotExists)
            {
                throw new DomainException("Os ingredientes devem ser existir");
            }
        }

        private static void AddIngredientsToRecipe(Recipe recipe, List<Ingredient> ingredients, IEnumerable<RecipeIngredientQueryRequestDto> requestIngredients)
        {
            foreach (var item in requestIngredients)
            {
                var ingredientReference = ingredients.First(x => x.Id == item.IngredientId);
                recipe.AddIngredient(ingredientReference);
            }
        }
    }
}