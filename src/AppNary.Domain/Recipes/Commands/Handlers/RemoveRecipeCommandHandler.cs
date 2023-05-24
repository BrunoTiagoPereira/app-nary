using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Commands.Responses;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using MediatR;

namespace AppNary.Domain.Recipes.Commands.Handlers
{
    public class RemoveRecipeCommandHandler : IRequestHandler<RemoveRecipeCommandRequest, RemoveRecipeCommandResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRecipeCommandHandler(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RemoveRecipeCommandResponse> Handle(RemoveRecipeCommandRequest request, CancellationToken cancellationToken)
        {
            var recipe = await GetRecipe(request.RecipeId);

            _recipeRepository.Remove(recipe);
            await _unitOfWork.CommitAsync();

            return new RemoveRecipeCommandResponse();
        }

        private async Task<Recipe> GetRecipe(Guid recipeId)
        {
            var recipe = await _recipeRepository.FindAsync(recipeId);

            if (recipe is null)
            {
                throw new DomainException("Receita não existe");
            }

            return recipe;
        }
    }
}