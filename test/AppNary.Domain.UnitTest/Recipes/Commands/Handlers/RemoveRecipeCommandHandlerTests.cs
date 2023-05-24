using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Handlers;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Repositories;
using AppNary.UnitTest.Abstractions.Fakers;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Commands.Handlers
{
    public class RemoveRecipeCommandHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly RecipeFaker _recipeFaker;

        private readonly UserFaker _userFaker;

        public RemoveRecipeCommandHandlerTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _recipeFaker = new RecipeFaker();
            _userFaker = new UserFaker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_repository()
        {
            FluentActions.Invoking(() => new RemoveRecipeCommandHandler(default(IRecipeRepository), _unitOfWorkMock.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_unit_of_work()
        {
            FluentActions.Invoking(() => new RemoveRecipeCommandHandler(_recipeRepositoryMock.Object, default(IUnitOfWork))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Should_throw_when_recipe_does_not_exists()
        {
            // Given
            var handler = new RemoveRecipeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object);

            // When
            await FluentActions.Invoking(() => handler.Handle(new RemoveRecipeCommandRequest { RecipeId = Guid.NewGuid() }, CancellationToken.None)).Should().ThrowAsync<DomainException>();

            // Then
            _recipeRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var recipe = _recipeFaker.Generate();
            var user = _userFaker.Generate();

            var handler = new RemoveRecipeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object);

            _recipeRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Guid>())).ReturnsAsync(recipe);

            // When
            await handler.Handle(new RemoveRecipeCommandRequest { RecipeId = recipe.Id }, CancellationToken.None);

            // Then
            _recipeRepositoryMock.Verify(x => x.FindAsync(recipe.Id));
            _recipeRepositoryMock.Verify(x => x.Remove(recipe));
            _unitOfWorkMock.Verify(x => x.CommitAsync());
        }
    }
}