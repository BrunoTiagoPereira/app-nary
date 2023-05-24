using AppNary.Domain.Users.Contracts;
using AppNary.Domain.Users.Entities;

namespace AppNary.Domain.Users.Managers
{
    public interface IUserAccessorManager
    {
        Guid GetCurrentUserId();
        void ThrowIfUserDontHasAccess(IUserRelated entity);
        Task<User> GetCurrentUser();
    }
}