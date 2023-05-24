using AppNary.Domain.Recipes.Entities;
using Bogus;

namespace AppNary.UnitTest.Abstractions.Fakers
{
    public class RecipeFaker : Faker<Recipe>
    {
        public RecipeFaker()
        {
            CustomInstantiator(f =>
            {
                var recipe = new Recipe(f.Lorem.Word(), f.Lorem.Sentence(5), new UserFaker().Generate(), f.Internet.Url());
                recipe.UpdateCanShow(true);
                return recipe;
            });
        }
    }
}