using AppNary.Core.DomainObjects;
using AppNary.Domain.Users.Entities;

namespace AppNary.Domain.Recipes.Entities
{
    public class Like : Entity
    {
        public Like(User user, Recipe recipe)
        {
            UpdateUser(user);
            UpdateRecipe(recipe);
        }

        protected Like() { }

        private void UpdateUser(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UserId = user.Id;
            User = user;
        }

        private void UpdateRecipe(Recipe recipe)
        {
            if (recipe is null)
            {
                throw new ArgumentNullException(nameof(recipe));
            }

            RecipeId = recipe.Id;
            Recipe = recipe;
        }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid RecipeId { get; private set; }
        public Recipe Recipe { get; private set; }
    }
}