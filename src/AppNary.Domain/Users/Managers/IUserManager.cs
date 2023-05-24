using AppNary.Domain.Users.Entities;

namespace AppNary.Domain.Users.Managers
{
    public interface IUserManager
    {
        string GenerateToken(User user);
    }
}