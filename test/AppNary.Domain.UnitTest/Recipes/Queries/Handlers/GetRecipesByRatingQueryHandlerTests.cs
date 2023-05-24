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
    public class GetRecipesByRatingQueryHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
        private readonly Mock<IUserAccessorManager> _userAcessorManagerMock;

        private readonly RecipeFaker _recipeFaker;

        public GetRecipesByRatingQueryHandlerTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _userAcessorManagerMock = new Mock<IUserAccessorManager>();
            _recipeFaker = new RecipeFaker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_repository()
        {
            FluentActions.Invoking(() => new GetRecipesByRatingQueryHandler(default(IRecipeRepository), _userAcessorManagerMock.Object)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var pageSize = 1;
            var pageIndex = 2;
            var query = "query";
            var totalResults = 1;
            var recipe = _recipeFaker.Generate();
            var user = new UserFaker().Generate();

            var pagedResult = new PagedResult<Recipe>
            {
                Items = new[] { recipe },
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query,
                TotalResults = totalResults
            };

            var handler = new GetRecipesByRatingQueryHandler(_recipeRepositoryMock.Object, _userAcessorManagerMock.Object);

            recipe.AddLike(user);
            _recipeRepositoryMock.Setup(x => x.GetRecipesByRatingAsync(pageSize, pageIndex, query,  null)).ReturnsAsync(pagedResult);

            // When
            var result = await handler.Handle(new GetRecipesByRatingQueryRequest { PageIndex = pageIndex, PageSize = pageSize, Query = query }, CancellationToken.None);

            // Then
            _recipeRepositoryMock.Verify(x => x.GetRecipesByRatingAsync(pageSize, pageIndex, query, null));
            result.Result.TotalResults.Should().Be(totalResults);
            result.Result.Query.Should().Be(query);
            result.Result.PageIndex.Should().Be(pageIndex);
            result.Result.PageSize.Should().Be(pageSize);
            result.Result.Items.First().Name.Should().Be(recipe.Name);
            result.Result.Items.First().Description.Should().Be(recipe.Description);
            result.Result.Items.First().LikesCount.Should().Be(recipe.Likes.Count);
        }
    }
}