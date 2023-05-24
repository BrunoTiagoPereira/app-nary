using AppNary.Core.Communication.Pipelines;
using AppNary.Core.Exceptions;
using AppNary.UnitTest.Abstractions.Fakes;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace AppNary.Core.UnitTest.Communication.Pipelines
{
    public class RequestValidationPipelineBehaviorTests
    {
        [Fact]
        public void Should_throw_when_request_is_not_valid()
        {
            // Given

            var validator = new Mock<IValidator<RequestFake>>();
            var request = new RequestFake();
            var requestDelegate = new Mock<RequestHandlerDelegate<ResponseFake>>();
            var validationResult = new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("property", "error") });
            var pipeline = new RequestValidationPipelineBehavior<RequestFake, ResponseFake>(validator.Object);

            validator.Setup(x => x.Validate(It.IsAny<IValidationContext>())).Returns(validationResult);

            // When

            var action = () => { pipeline.Handle(request, requestDelegate.Object, CancellationToken.None).GetAwaiter().GetResult(); };

            action.Should().Throw<DomainException>().And
                .ValidationFailuresMessages.Should().ContainSingle().Which
                .Should().Be("error");
        }
    }
}