using AppNary.Domain.Recipes.Entities;
using Bogus;

namespace AppNary.UnitTest.Abstractions.Fakers
{
    public class IngredientFaker : Faker<Ingredient>
    {
        public IngredientFaker()
        {
            CustomInstantiator((f) =>
            {
                return new Ingredient(f.Lorem.Sentence(2), f.Lorem.Letter(2), f.Lorem.Letter(30));
            });
        }
    }
}