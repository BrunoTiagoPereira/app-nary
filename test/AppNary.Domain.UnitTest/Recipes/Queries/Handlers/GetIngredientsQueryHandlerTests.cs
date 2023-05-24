using AppNary.Core.Application.Pagination;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Queries.Handlers;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Repositories;
using AppNary.UnitTest.Abstractions.Fakers;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Commands.Handlers
{
    public class GetIngredientsQueryHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;

        private readonly IngredientFaker _ingredientFaker;

        public GetIngredientsQueryHandlerTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _ingredientFaker = new IngredientFaker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_repository()
        {
            FluentActions.Invoking(() => new GetIngredientsQueryHandler(default(IRecipeRepository))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var pageSize = 1;
            var pageIndex = 2;
            var query = "query";
            var totalResults = 1;
            var ingredient = _ingredientFaker.Generate();

            var pagedResult = new PagedResult<Ingredient>
            {
                Items = new[] { ingredient },
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query,
                TotalResults = totalResults
            };

            var handler = new GetIngredientsQueryHandler(_recipeRepositoryMock.Object);

            _recipeRepositoryMock.Setup(x => x.GetIngredientsAsync(pageSize, pageIndex, query)).ReturnsAsync(pagedResult);

            // When
            var result = await handler.Handle(new GetIngredientsQueryRequest { PageIndex = pageIndex, PageSize = pageSize, Query = query }, CancellationToken.None);

            // Then
            _recipeRepositoryMock.Verify(x => x.GetIngredientsAsync(pageSize, pageIndex, query));
            result.Result.TotalResults.Should().Be(totalResults);
            result.Result.Query.Should().Be(query);
            result.Result.PageIndex.Should().Be(pageIndex);
            result.Result.PageSize.Should().Be(pageSize);
            result.Result.Items.First().Id.Should().Be(ingredient.Id);
            result.Result.Items.First().Name.Should().Be(ingredient.Name);
            result.Result.Items.First().UnitOfMeasure.Should().Be(ingredient.UnitOfMeasure);
            result.Result.Items.First().SvgIcon.Should().Be(ingredient.SvgIcon);
        }
    }
}