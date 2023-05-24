using AppNary.Core.Application.Pagination;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Queries.Handlers;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Users.Managers;
using AppNary.UnitTest.Abstractions.Fakers;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Commands.Handlers
{
    public class GetRecipesByIngredientsQueryHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
        private readonly Mock<IUserAccessorManager> _userAccessorManagerMock;

        private readonly RecipeFaker _recipeFaker;
        private readonly IngredientFaker _ingredientFaker;

        public GetRecipesByIngredientsQueryHandlerTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _userAccessorManagerMock = new Mock<IUserAccessorManager>();
            _recipeFaker = new RecipeFaker();
            _ingredientFaker = new IngredientFaker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_repository()
        {
            FluentActions.Invoking(() => new GetRecipesByIngredientsQueryHandler(default(IRecipeRepository), _userAccessorManagerMock.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var pageSize = 1;
            var pageIndex = 2;
            var ingredient = _ingredientFaker.Generate();
            var totalResults = 1;
            var recipe = _recipeFaker.Generate();
            var user = new UserFaker().Generate();
            var ingredientsIds = new[] { ingredient.Id };

            var pagedResult = new PagedResult<Recipe>
            {
                Items = new[] { recipe },
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalResults = totalResults
            };

            var handler = new GetRecipesByIngredientsQueryHandler(_recipeRepositoryMock.Object, _userAccessorManagerMock.Object);

            recipe.AddLike(user);
            _recipeRepositoryMock.Setup(x => x.GetRecipesByIngredientsIdsAsync(ingredientsIds, pageSize, pageIndex)).ReturnsAsync(pagedResult);

            // When
            var result = await handler.Handle(new GetRecipesByIngredientsQueryRequest { PageIndex = pageIndex, PageSize = pageSize, IngredientsIds = ingredientsIds }, CancellationToken.None);

            // Then
            _recipeRepositoryMock.Verify(x => x.GetRecipesByIngredientsIdsAsync(ingredientsIds, pageSize, pageIndex));
            result.Result.TotalResults.Should().Be(totalResults);
            result.Result.PageIndex.Should().Be(pageIndex);
            result.Result.PageSize.Should().Be(pageSize);
            result.Result.Items.First().Name.Should().Be(recipe.Name);
            result.Result.Items.First().Description.Should().Be(recipe.Description);
            result.Result.Items.First().LikesCount.Should().Be(recipe.Likes.Count);
        }
    }
}