using AppNary.Data;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace AppNary.UnitTest.Abstractions.Fakes
{
    public class DatabaseContextFaker : Faker<DatabaseContext>
    {
        public DatabaseContextFaker()
        {
            CustomInstantiator(x =>
            {
                var builder = new DbContextOptionsBuilder<DatabaseContext>();
                builder.UseInMemoryDatabase(x.Random.Guid().ToString());

                return new DatabaseContext(builder.Options);
            });
        }
    }
}