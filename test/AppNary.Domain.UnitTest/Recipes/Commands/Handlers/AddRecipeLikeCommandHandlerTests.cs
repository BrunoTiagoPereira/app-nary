using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Users.Commands.Handlers;
using AppNary.Domain.Users.Managers;
using AppNary.UnitTest.Abstractions.Fakers;
using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Commands.Handlers
{
    public class AddRecipeLikeCommandHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly Mock<IUserAccessorManager> _userAccessorManager;

        private readonly Faker _faker;

        private readonly RecipeFaker _recipeFaker;

        private readonly UserFaker _userFaker;

        private readonly IngredientFaker _ingredientFaker;

        public AddRecipeLikeCommandHandlerTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userAccessorManager = new Mock<IUserAccessorManager>();
            _recipeFaker = new RecipeFaker();
            _userFaker = new UserFaker();
            _faker = new Faker();
            _ingredientFaker = new IngredientFaker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_repository()
        {
            FluentActions.Invoking(() => new AddRecipeLikeCommandHandler(default(IRecipeRepository), _unitOfWorkMock.Object, _userAccessorManager.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_unit_of_work()
        {
            FluentActions.Invoking(() => new AddRecipeLikeCommandHandler(_recipeRepositoryMock.Object, default(IUnitOfWork), _userAccessorManager.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_user_accessor_manager()
        {
            FluentActions.Invoking(() => new AddRecipeLikeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, default(IUserAccessorManager))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Should_throw_when_recipe_does_not_exists()
        {
            // Given
            var handler = new AddRecipeLikeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, _userAccessorManager.Object);

            // When
            await FluentActions.Invoking(() => handler.Handle(new AddRecipeLikeCommandRequest { RecipeId = Guid.NewGuid() }, CancellationToken.None)).Should().ThrowAsync<DomainException>();

            // Then
            _recipeRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var recipe = _recipeFaker.Generate();
            var user = _userFaker.Generate();

            var handler = new AddRecipeLikeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, _userAccessorManager.Object);

            _userAccessorManager.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _recipeRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Guid>())).ReturnsAsync(recipe);


            // When
            await handler.Handle(new AddRecipeLikeCommandRequest { RecipeId = recipe.Id }, CancellationToken.None);

            // Then
            _userAccessorManager.Verify(x => x.GetCurrentUser());
            _recipeRepositoryMock.Verify(x => x.FindAsync(recipe.Id));
            _recipeRepositoryMock.Verify(x => x.Update(recipe));
            _unitOfWorkMock.Verify(x => x.CommitAsync());
        }
    }
}