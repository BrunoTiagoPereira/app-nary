using AppNary.Domain.Recipes.Entities;
using Bogus;

namespace AppNary.UnitTest.Abstractions.Fakers
{
    public class LikeFaker : Faker<Like>
    {
        public LikeFaker()
        {
            CustomInstantiator((f) =>
            {
                return new Like(new UserFaker().Generate(), new RecipeFaker().Generate());
            });
        }
    }
}