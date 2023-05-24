using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Commands.Responses;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Recipes.Services;
using MediatR;

namespace AppNary.Domain.Users.Commands.Handlers
{
    public class UploadRecipeImageCommandHandler : IRequestHandler<UploadRecipeImageCommandRequest, UploadRecipeImageCommandResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IImageStorageManager _imageStorageManager;

        public UploadRecipeImageCommandHandler(IRecipeRepository recipeRepository, IUnitOfWork uow, IImageStorageManager imageStorageManager)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _imageStorageManager = imageStorageManager ?? throw new ArgumentNullException(nameof(imageStorageManager));
        }

        public async Task<UploadRecipeImageCommandResponse> Handle(UploadRecipeImageCommandRequest request, CancellationToken cancellationToken)
        {
            var recipe = await GetRecipe(request.RecipeId);

            var recipeImageUrl = await _imageStorageManager.Save(recipe.Id, request.Image);
            recipe.UpdateImageUrl(recipeImageUrl);

            _recipeRepository.Update(recipe);
            await _uow.CommitAsync();

            return new UploadRecipeImageCommandResponse { ImageUrl = recipeImageUrl };
        }

        private async Task<Recipe> GetRecipe(Guid recipeId)
        {
            var recipe = await _recipeRepository.FindAsync(recipeId);

            if (recipe is null)
            {
                throw new DomainException("A receita não existe");
            }

            return recipe;
        }
    }
}