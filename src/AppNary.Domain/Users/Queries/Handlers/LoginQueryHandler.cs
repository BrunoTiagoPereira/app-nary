using AppNary.Core.Exceptions;
using AppNary.Core.ValueObjects;
using AppNary.Domain.Users.Managers;
using AppNary.Domain.Users.Repositories;
using MediatR;
using ProductsPricing.Domain.Users.Queries.Requests;
using ProductsPricing.Domain.Users.Queries.Responses;

namespace AppNary.Domain.Users.Queries.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQueryRequest, LoginQueryResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;

        public LoginQueryHandler(IUserRepository userRepository, IUserManager userManager)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<LoginQueryResponse> Handle(LoginQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByUserNameAsync(request.UserName);

            if (user is null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            var isPasswordInvalid = new Password(request.Password) != user.Password;

            if (isPasswordInvalid)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            return new LoginQueryResponse { Token = _userManager.GenerateToken(user) };
        }
    }
}