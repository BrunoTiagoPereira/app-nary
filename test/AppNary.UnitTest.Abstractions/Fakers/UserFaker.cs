using AppNary.Domain.Users.Entities;
using Bogus;

namespace AppNary.UnitTest.Abstractions.Fakers
{
    public class UserFaker : Faker<User>
    {
        public UserFaker()
        {
            CustomInstantiator(f =>
            {
                return new User(f.Internet.UserName(), f.Internet.Password());
            });
        }
    }
}