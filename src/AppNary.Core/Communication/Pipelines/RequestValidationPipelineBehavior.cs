using AppNary.Core.Exceptions;
using FluentValidation;
using MediatR;

namespace AppNary.Core.Communication.Pipelines
{
    public class RequestValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public RequestValidationPipelineBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validator
                .Validate(context)
                .Errors
                .Where(f => f is not null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new DomainException(failures);
            }

            return next();
        }
    }
}