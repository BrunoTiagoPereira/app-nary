using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Users.Commands.Requests;
using AppNary.Domain.Users.Commands.Responses;
using AppNary.Domain.Users.Entities;
using AppNary.Domain.Users.Repositories;
using MediatR;

namespace AppNary.Domain.Users.Commands.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork uow)
        {
            _userRepository = userRepository;
            _uow = uow;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var userNameIsTaken = await _userRepository.UserNameIsTakenAsync(request.UserName);

            if (userNameIsTaken)
            {
                throw new DomainException("O nome de usuário está em uso.");
            }

            await _userRepository.AddAsync(new User(request.UserName, request.Password));
            await _uow.CommitAsync();

            return new CreateUserCommandResponse();
        }
    }
}