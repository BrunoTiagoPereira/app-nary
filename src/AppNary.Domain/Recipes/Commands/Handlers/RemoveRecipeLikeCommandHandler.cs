﻿using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Commands.Responses;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Users.Managers;
using MediatR;

namespace AppNary.Domain.Users.Commands.Handlers
{
    public class RemoveRecipeLikeCommandHandler : IRequestHandler<RemoveRecipeLikeCommandRequest, RemoveRecipeLikeCommandResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IUserAccessorManager _userAccessorManager;

        public RemoveRecipeLikeCommandHandler(IRecipeRepository recipeRepository, IUnitOfWork uow, IUserAccessorManager userAccessorManager)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userAccessorManager = userAccessorManager ?? throw new ArgumentNullException(nameof(userAccessorManager));
        }

        public async Task<RemoveRecipeLikeCommandResponse> Handle(RemoveRecipeLikeCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userAccessorManager.GetCurrentUser();
            var recipe = await GetRecipe(request.RecipeId);

            recipe.RemoveLike(user);

            _recipeRepository.Update(recipe);
            await _uow.CommitAsync();

            return new RemoveRecipeLikeCommandResponse();
        }

        private async Task<Recipe> GetRecipe(Guid recipeId)
        {
            var recipe = await _recipeRepository.GetRecipe(recipeId);

            if (recipe is null)
            {
                throw new DomainException("A receita não existe");
            }

            return recipe;
        }
    }
}