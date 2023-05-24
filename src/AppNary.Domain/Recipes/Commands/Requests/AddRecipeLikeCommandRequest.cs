using AppNary.Domain.Recipes.Commands.Responses;
using MediatR;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class AddRecipeLikeCommandRequest : IRequest<AddRecipeLikeCommandResponse>
    {
        public Guid RecipeId { get; set; }
    }
}