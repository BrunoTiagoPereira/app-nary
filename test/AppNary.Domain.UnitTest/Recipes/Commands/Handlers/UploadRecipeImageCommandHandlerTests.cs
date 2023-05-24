using AppNary.Core.Exceptions;
using AppNary.Core.Transaction;
using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Recipes.Services;
using AppNary.Domain.Users.Commands.Handlers;
using AppNary.UnitTest.Abstractions.Fakers;
using AppNary.UnitTest.Abstractions.Fakes;
using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Commands.Handlers
{
    public class UploadRecipeImageCommandHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly Mock<IImageStorageManager> _imageStorageManagerMock;

        private readonly Faker _faker;

        private readonly RecipeFaker _recipeFaker;

        public UploadRecipeImageCommandHandlerTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _imageStorageManagerMock = new Mock<IImageStorageManager>();
            _recipeFaker = new RecipeFaker();
            _faker = new Faker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_repository()
        {
            FluentActions.Invoking(() => new UploadRecipeImageCommandHandler(default(IRecipeRepository), _unitOfWorkMock.Object, _imageStorageManagerMock.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_unit_of_work()
        {
            FluentActions.Invoking(() => new UploadRecipeImageCommandHandler(_recipeRepositoryMock.Object, default(IUnitOfWork), _imageStorageManagerMock.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_image_storage_manager()
        {
            FluentActions.Invoking(() => new UploadRecipeImageCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, default(IImageStorageManager))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Should_throw_when_recipe_does_not_exists()
        {
            // Given
            var handler = new UploadRecipeImageCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, _imageStorageManagerMock.Object);

            // When
            await FluentActions.Invoking(() => handler.Handle(new UploadRecipeImageCommandRequest { RecipeId = Guid.NewGuid() }, CancellationToken.None)).Should().ThrowAsync<DomainException>();

            // Then
            _recipeRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var recipe = _recipeFaker.Generate();
            var imageUrl = _faker.Internet.Url();
            var formFile = new FormFileFaker();

            var handler = new UploadRecipeImageCommandHandler(_recipeRepositoryMock.Object, _unitOfWorkMock.Object, _imageStorageManagerMock.Object);

            _imageStorageManagerMock.Setup(x => x.Save(recipe.Id, formFile)).ReturnsAsync(imageUrl);
            _recipeRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Guid>())).ReturnsAsync(recipe);

            // When
            var result = await handler.Handle(new UploadRecipeImageCommandRequest { RecipeId = recipe.Id, Image = formFile }, CancellationToken.None);

            // Then
            result.ImageUrl.Should().Be(imageUrl);
            _imageStorageManagerMock.Verify(x => x.Save(recipe.Id, formFile));
            _recipeRepositoryMock.Verify(x => x.FindAsync(recipe.Id));
            _recipeRepositoryMock.Verify(x => x.Update(recipe));
            _unitOfWorkMock.Verify(x => x.CommitAsync());
        }
    }
}