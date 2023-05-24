using AppNary.Domain.Users.Entities;

namespace AppNary.Domain.Users.Contracts
{
    public interface IUserRelated
    {
        User User { get; }
        Guid UserId { get; }
    }
}