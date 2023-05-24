using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Dtos;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Recipes.Services;
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
    public class CreateRecipeCommandHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly Mock<IUserAccessorManager> _userAccessorManager;

        private readonly Faker _faker;

        private readonly RecipeFaker _recipeFaker;

        private readonly UserFaker _userFaker;

        private readonly IngredientFaker _ingredientFaker;

        public CreateRecipeCommandHandlerTests()
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
            FluentActions.Invoking(() => new CreateRecipeCommandHandler(default(IRecipeRepository), _unitOfWorkMock.Object, _userAccessorManager.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_unit_of_work()
        {
            FluentActions.Invoking(() => new CreateRecipeCommandHandler(_recipeRepositoryMock.Object, default(IUnitOfWork), _userAccessorManager.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_user_accessor_manager()
        {
            FluentActions.Invoking(() => new CreateRecipeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, default(IUserAccessorManager))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Should_throw_when_any_ingredient_does_not_exists()
        {
            // Given
            var recipe = _recipeFaker.Generate();
            var ingredientId = Guid.NewGuid();
            var user = _userFaker.Generate();
            var quantity = _faker.Random.Number(min: 1);

            var ingredientDto = new RecipeIngredientQueryRequestDto { IngredientId = ingredientId };
            var handler = new CreateRecipeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, _userAccessorManager.Object);

            _userAccessorManager.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _recipeRepositoryMock.Setup(x => x.AnyIngredientDoesNotExistsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(true);

            // When
            await FluentActions.Invoking(() => handler.Handle(new CreateRecipeCommandRequest { Name = recipe.Name, Description = recipe.Description, Ingredients = new List<RecipeIngredientQueryRequestDto> { ingredientDto } }, CancellationToken.None)).Should().ThrowAsync<DomainException>();

            // Then
            _recipeRepositoryMock.Verify(x => x.AnyIngredientDoesNotExistsAsync(It.IsAny<IEnumerable<Guid>>()));
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var recipe = _recipeFaker.Generate();
            var ingredient = _ingredientFaker.Generate();
            var user = _userFaker.Generate();
            var quantity = _faker.Random.Number(min: 1);

            var ingredientDto = new RecipeIngredientQueryRequestDto { IngredientId = ingredient.Id };
            var handler = new CreateRecipeCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, _userAccessorManager.Object);

            _userAccessorManager.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _recipeRepositoryMock.Setup(x => x.AnyIngredientDoesNotExistsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(false);
            _recipeRepositoryMock.Setup(x => x.GetIngredientsFromIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(new List<Ingredient> { ingredient });

            // When
            var result = await handler.Handle(new CreateRecipeCommandRequest { Name = recipe.Name, Description = recipe.Description, Ingredients = new List<RecipeIngredientQueryRequestDto> { ingredientDto } }, CancellationToken.None);

            // Then
            result.RecipeId.Should().NotBeEmpty();
            _userAccessorManager.Verify(x => x.GetCurrentUser());
            _recipeRepositoryMock.Verify(x => x.AddAsync(It.Is<Recipe>(y => y.Name == recipe.Name && y.Description == recipe.Description && y.Ingredients.First().IngredientId == ingredient.Id)));
            _recipeRepositoryMock.Verify(x => x.GetIngredientsFromIdsAsync(It.IsAny<IEnumerable<Guid>>()));
            _unitOfWorkMock.Verify(x => x.CommitAsync());
        }
    }
}