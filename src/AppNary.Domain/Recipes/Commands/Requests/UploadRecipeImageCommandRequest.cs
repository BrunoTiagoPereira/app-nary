using AppNary.Domain.Recipes.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class UploadRecipeImageCommandRequest : IRequest<UploadRecipeImageCommandResponse>
    {
        public Guid RecipeId { get; set; }

        public IFormFile Image { get; set; }
    }

}