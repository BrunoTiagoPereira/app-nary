using AppNary.Core.DomainObjects;
using AppNary.Core.Exceptions;

namespace AppNary.Domain.Recipes.Entities
{
    public class RecipeIngredient : Entity
    {
        public RecipeIngredient(Recipe recipe, Ingredient ingredient)
        {
            UpdateRecipe(recipe);
            UpdateIngredient(ingredient);
        }

        protected RecipeIngredient() {}

        public Guid RecipeId { get; private set; }
        public Recipe Recipe { get; private set; }

        public Guid IngredientId { get; private set; }
        public Ingredient Ingredient { get; private set; }

        private void UpdateRecipe(Recipe recipe)
        {
            if (recipe is null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }

            RecipeId = recipe.Id;
            Recipe = recipe;
        }

        private void UpdateIngredient(Ingredient ingredient)
        {
            if (ingredient is null)
            {
                throw new ArgumentNullException(nameof(ingredient));
            }

            IngredientId = ingredient.Id;
            Ingredient = ingredient;
        }
    }
}