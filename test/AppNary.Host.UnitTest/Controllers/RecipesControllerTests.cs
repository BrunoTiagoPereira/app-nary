using AppNary.Domain.Recipes.Commands.Requests;
using AppNary.Domain.Recipes.Commands.Responses;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Queries.Responses;
using AppNary.Host.Controllers;
using Azure;
using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppNary.Host.UnitTest.Controllers
{
    public class RecipesControllerTests
    {
        private readonly RecipesController _recipeController;
        private readonly Mock<IMediator> _requestSender;
        private readonly Faker _faker;

        public RecipesControllerTests()
        {
            _requestSender = new Mock<IMediator>();
            _recipeController = new RecipesController(_requestSender.Object);
            _faker = new Faker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_sender()
        {
            // Given / When
            var action = () => new UsersController(null);

            // Then
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_get_recipe()
        {
            // Given
            var request = new GetRecipeQueryRequest();
            var recipeId = Guid.NewGuid();
            var response = new GetRecipeQueryResponse();

            _requestSender.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            // When
            var result = await _recipeController.GetRecipe(recipeId);


            // Then
            result.Should().NotBeNull();
            _requestSender.Verify(x => x.Send(It.Is<GetRecipeQueryRequest>(y => y.RecipeId == recipeId), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_create_recipe()
        {
            // Given
            var request = new CreateRecipeCommandRequest();
            var recipeId = Guid.NewGuid();
            var response = new CreateRecipeCommandResponse { RecipeId = recipeId };

            _requestSender.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            // When
            var result = await _recipeController.CreateRecipeAsync(request);


            // Then
            result.Should().NotBeNull();
            result.Data.Should().Be(response);
            _requestSender.Verify(x => x.Send(request, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_update_recipe()
        {
            // Given
            var request = new UpdateRecipeCommandRequest();
            var recipeId = Guid.NewGuid();
            var response = new UpdateRecipeCommandResponse();

            _requestSender.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            // When
            var result = await _recipeController.UpdateRecipeAsync(request);


            // Then
            result.Should().NotBeNull();
            _requestSender.Verify(x => x.Send(request, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_upload_recipe_image()
        {
            // Given
            var request = new UploadRecipeImageCommandRequest();
            var imageUrl = _faker.Internet.Url();
            var response = new UploadRecipeImageCommandResponse { ImageUrl = imageUrl };

            _requestSender.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            // When
            var result = await _recipeController.UploadRecipeImageAsync(request);

            // Then
            result.Should().NotBeNull();
            result.Data.Should().Be(response);
            _requestSender.Verify(x => x.Send(request, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_remove_recipe()
        {
            // Given / When
            var recipeId = Guid.NewGuid();

            var result = await _recipeController.RemoveRecipeAsync(recipeId);

            // Then
            result.Should().NotBeNull();
            _requestSender.Verify(x => x.Send(It.Is<RemoveRecipeCommandRequest>(y => y.RecipeId == recipeId), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_get_recipes_by_rating()
        {
            // Given / When
            var pageSize = 1;
            var pageIndex = 2;
            var query = "query";

            var result = await _recipeController.GetRecipesByRating(pageSize, pageIndex, query);

            // Then
            result.Should().NotBeNull();
            _requestSender.Verify(x => x.Send(It.Is<GetRecipesByRatingQueryRequest>(y => y.PageSize == pageSize && y.PageIndex == pageIndex && y.Query == query), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_get_ingredients()
        {
            // Given / When
            var pageSize = 1;
            var pageIndex = 2;
            var query = "query";

            var result = await _recipeController.GetIngredients(pageSize, pageIndex, query);

            // Then
            result.Should().NotBeNull();
            _requestSender.Verify(x => x.Send(It.Is<GetIngredientsQueryRequest>(y => y.PageSize == pageSize && y.PageIndex == pageIndex && y.Query == query), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_get_recipes_by_ingredients()
        {
            // Given / When
            var pageSize = 1;
            var pageIndex = 2;
            var ingredientsIds = new[] {Guid.NewGuid()};

            var result = await _recipeController.GetRecipesByIngredients(ingredientsIds, pageSize, pageIndex);

            // Then
            result.Should().NotBeNull();
            _requestSender.Verify(x => x.Send(It.Is<GetRecipesByIngredientsQueryRequest>(y => y.PageSize == pageSize && y.PageIndex == pageIndex && y.IngredientsIds == ingredientsIds), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_like()
        {
            // Given / When
            var request = new AddRecipeLikeCommandRequest();
            var result = await _recipeController.Like(request);

            // Then
            result.Should().NotBeNull();
            _requestSender.Verify(x => x.Send(request, It.IsAny<CancellationToken>()));
        }
    }
}