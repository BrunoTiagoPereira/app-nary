using AppNary.Core.Data.Repositories;
using AppNary.Domain.Users.Entities;
using AppNary.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppNary.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public Task<bool> UserNameIsTakenAsync(string userName)
        {
            return Set.AnyAsync(x => x.UserName == userName);
        }

        public Task<User?> FindByUserNameAsync(string userName)
        {
            return Set
                .SingleOrDefaultAsync(x => x.UserName == userName)
                ;
        }
    }
}