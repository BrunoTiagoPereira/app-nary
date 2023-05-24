using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Data;
using AppNary.Data.Repositories;
using AppNary.Domain.Users.Commands.Handlers;
using AppNary.Domain.Users.Commands.Requests;
using AppNary.Domain.Users.Entities;
using AppNary.UnitTest.Abstractions.Fakes;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppNary.Domain.UnitTest.Users.Commands.Handlers
{
    public class CreateUserComandHandlerTests
    {
        private readonly DatabaseContext _context;
        private readonly UserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Faker _faker;

        public CreateUserComandHandlerTests()
        {
            _context = new DatabaseContextFaker().Generate();
            _userRepository = new(_context);
            _unitOfWork = new UnitOfWork(_context, new Mock<IServiceScopeFactory>().Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Should_throw_when_email_is_taken()
        {
            // Given / When
            var user = new User(_faker.Internet.UserName(), _faker.Internet.Password(8));
            var handler = GetHandler();
            var action = async () => await handler.Handle(new CreateUserCommandRequest
            {
                UserName = user.UserName,
                Password = user.Password.Hash,
                PasswordConfirmation = user.Password.Hash
            }, CancellationToken.None);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            // Then
            await action.Should().ThrowAsync<DomainException>();
        }

        [Fact]
        public async Task Should_handle()
        {
            // Given
            var handler = GetHandler();
            var userName = _faker.Internet.UserName();
            var password = _faker.Internet.Password();

            // When
            await handler.Handle(new CreateUserCommandRequest
            {
                UserName = userName,
                Password = password,
                PasswordConfirmation = password
            }, CancellationToken.None);

            // Then
            var result = await _userRepository.FindByUserNameAsync(userName);
            result.Should().NotBeNull();
        }

        public CreateUserCommandHandler GetHandler() => new(_userRepository, _unitOfWork);
    }
}