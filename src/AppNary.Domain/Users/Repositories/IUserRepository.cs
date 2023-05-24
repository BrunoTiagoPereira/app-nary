using AppNary.Domain.Users.Entities;
using ProductsPricing.Core.Data;

namespace AppNary.Domain.Users.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UserNameIsTakenAsync(string userName);

        Task<User?> FindByUserNameAsync(string userName);
    }
}