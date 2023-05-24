using AppNary.Domain.Users.Commands.Responses;
using MediatR;

namespace AppNary.Domain.Users.Commands.Requests
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}