using AppNary.Core.Application.Pagination;
using AppNary.Core.Exceptions;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Queries.Handlers;
using AppNary.Domain.Recipes.Queries.Requests;
using AppNary.Domain.Recipes.Repositories;
using AppNary.UnitTest.Abstractions.Fakers;
using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Commands.Handlers
{
    public class GetRecipeQueryHandlerTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;

        private readonly Faker _faker;
        private readonly RecipeFaker _recipeFaker;
        private readonly UserFaker _userFaker;
        private readonly IngredientFaker _ingredientFaker;

        public GetRecipeQueryHandlerTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _faker = new Faker();
            _recipeFaker = new RecipeFaker();
            _userFaker = new UserFaker();
            _ingredientFaker = new IngredientFaker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_repository()
        {
            FluentActions.Invoking(() => new GetRecipeQueryHandler(default(IRecipeRepository))).Should().Throw<ArgumentNullException>();
        }
        [Fact]
        public async Task Should_throw_when_recipe_does_not_exists()
        {
            // Given
            var handler = new GetRecipeQueryHandler(_recipeRepositoryMock.Object);

            // When / Then
            await FluentActions.Invoking(() => handler.Handle(new GetRecipeQueryRequest { RecipeId = Guid.NewGuid() }, CancellationToken.None)).Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async void Should_handle()
        {
            // Given
            var recipe = _recipeFaker.Generate();
            var ingredient = _ingredientFaker.Generate();
            var quantity = _faker.Random.Number(min: 1);
            var user = _userFaker.Generate();

            recipe.AddIngredient(ingredient);
            recipe.AddLike(user);

            var handler = new GetRecipeQueryHandler(_recipeRepositoryMock.Object);

            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipe.Id)).ReturnsAsync(recipe);

            // When
            var result = await handler.Handle(new GetRecipeQueryRequest { RecipeId = recipe.Id }, CancellationToken.None);

            // Then
            _recipeRepositoryMock.Verify(x => x.GetRecipe(recipe.Id));
            result.Recipe.Id.Should().Be(recipe.Id);
            result.Recipe.Name.Should().Be(recipe.Name);
            result.Recipe.Description.Should().Be(recipe.Description);
            result.Recipe.LikesCount.Should().Be(recipe.Likes.Count);
            result.Recipe.Ingredients.Should().ContainSingle();
            result.Recipe.Ingredients.First().Id.Should().Be(ingredient.Id);
            result.Recipe.Ingredients.First().Name.Should().Be(ingredient.Name);
            result.Recipe.Ingredients.First().SvgIcon.Should().Be(ingredient.SvgIcon);
        }
    }
}