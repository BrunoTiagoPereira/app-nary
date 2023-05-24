using AppNary.Core.Exceptions;
using AppNary.Domain.Users.Contracts;
using AppNary.Domain.Users.Entities;
using AppNary.Domain.Users.Managers;
using AppNary.Domain.Users.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ProductsPricing.Domain.Users.Managers
{
    public class UserAccessorManager : IUserAccessorManager
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserRepository _userRepository;

        public UserAccessorManager(IHttpContextAccessor accessor, IUserRepository userRepository)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public Task<User> GetCurrentUser()
        {
            return _userRepository.FindAsync(GetCurrentUserId());
        }

        public Guid GetCurrentUserId()
        {
            return Guid.Parse(_accessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

        public void ThrowIfUserDontHasAccess(IUserRelated entity)
        {
            if(GetCurrentUserId() != entity.UserId)
            {
                throw new NotAuthorizedException("Usuário não autorizado");
            }
        }
    }
}