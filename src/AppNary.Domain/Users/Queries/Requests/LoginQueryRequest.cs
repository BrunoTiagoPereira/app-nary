using MediatR;
using ProductsPricing.Domain.Users.Queries.Responses;

namespace ProductsPricing.Domain.Users.Queries.Requests
{
    public class LoginQueryRequest : IRequest<LoginQueryResponse>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}