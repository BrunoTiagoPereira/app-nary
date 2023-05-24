using AppNary.Core.Exceptions;
using AppNary.Host.Middlewares;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AppNary.Host.UnitTest.Middlewares
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task Should_handle_DomainException()
        {
            // given
            var middleware = new ExceptionHandlingMiddleware();
            var context = new DefaultHttpContext();

            // when
            var action = () => middleware.InvokeAsync(context, (_) => throw new DomainException("Error"));

            // then
            await action.Should().NotThrowAsync();
            context.Response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Should_handle_EntityNotFoundException()
        {
            // given
            var middleware = new ExceptionHandlingMiddleware();
            var context = new DefaultHttpContext();

            // when
            var action = () => middleware.InvokeAsync(context, (_) => throw new NotFoundException("Error"));

            // then
            await action.Should().NotThrowAsync();
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Should_handle_Exception()
        {
            // given
            var middleware = new ExceptionHandlingMiddleware();
            var context = new DefaultHttpContext();

            // when
            var action = () => middleware.InvokeAsync(context, (_) => throw new Exception());

            // then
            await action.Should().NotThrowAsync();
            context.Response.StatusCode.Should().Be(500);
        }
    }
}