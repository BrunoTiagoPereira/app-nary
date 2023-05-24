using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Commands.Responses;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Users.Managers;
using MediatR;

namespace AppNary.Domain.Users.Commands.Handlers
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommandRequest, CreateRecipeCommandResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IUserAccessorManager _userAccessorManager;

        public CreateRecipeCommandHandler(IRecipeRepository recipeRepository, IUnitOfWork uow, IUserAccessorManager userAccessorManager)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userAccessorManager = userAccessorManager ?? throw new ArgumentNullException(nameof(userAccessorManager));
        }

        public async Task<CreateRecipeCommandResponse> Handle(CreateRecipeCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userAccessorManager.GetCurrentUser();
            var recipe = new Recipe(request.Name, request.Description, user);

            await ThrowIfAnyIngredientDoesNotExists(request);

            var requestIngredients = await _recipeRepository.GetIngredientsFromIdsAsync(request.Ingredients.Select(x => x.IngredientId));

            AddIngredientsToRecipe(recipe, requestIngredients, request);

            await _recipeRepository.AddAsync(recipe);
            await _uow.CommitAsync();

            return new CreateRecipeCommandResponse { RecipeId = recipe.Id };
        }

        private async Task ThrowIfAnyIngredientDoesNotExists(CreateRecipeCommandRequest request)
        {
            var requestIngredientsIds = request.Ingredients.Select(x => x.IngredientId);

            var anyIngredientDoesNotExists = await _recipeRepository.AnyIngredientDoesNotExistsAsync(requestIngredientsIds);

            if (anyIngredientDoesNotExists)
            {
                throw new DomainException("Os ingredientes devem ser existir");
            }
        }

        private static void AddIngredientsToRecipe(Recipe recipe, List<Ingredient> requestIngredients, CreateRecipeCommandRequest request)
        {
            foreach (var item in request.Ingredients)
            {
                var ingredientReference = requestIngredients.First(x => x.Id == item.IngredientId);
                recipe.AddIngredient(ingredientReference);
            }
        }
    }
}