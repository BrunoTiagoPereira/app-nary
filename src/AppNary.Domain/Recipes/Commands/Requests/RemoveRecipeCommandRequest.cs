using AppNary.Domain.Recipes.Commands.Responses;
using MediatR;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class RemoveRecipeCommandRequest : IRequest<RemoveRecipeCommandResponse>
    {
        public Guid RecipeId { get; set; }
    }
}