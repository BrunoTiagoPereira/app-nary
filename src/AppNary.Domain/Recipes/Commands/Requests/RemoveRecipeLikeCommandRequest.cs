using AppNary.Domain.Recipes.Commands.Responses;
using MediatR;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class RemoveRecipeLikeCommandRequest : IRequest<RemoveRecipeLikeCommandResponse>
    {
        public Guid RecipeId { get; set; }
    }
}