using AppNary.Domain.Recipes.Commands.Responses;
using AppNary.Domain.Recipes.Dtos;
using MediatR;

namespace AppNary.Domain.Recipes.Commands.Requests
{
    public class CreateRecipeCommandRequest : IRequest<CreateRecipeCommandResponse>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<RecipeIngredientQueryRequestDto> Ingredients { get; set; }
    }
}