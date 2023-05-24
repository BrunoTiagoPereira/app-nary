using AppNary.Data.Repositories;
using AppNary.UnitTest.Abstractions.Fakers;
using AppNary.UnitTest.Abstractions.Fakes;
using Bogus;
using FluentAssertions;
using Xunit;

namespace AppNary.Data.UnitTest.Repositories
{
    public class RecipeRepositoryTests
    {
        private readonly DatabaseContext _dbContext;
        private readonly RecipeFaker _recipeFaker;
        private readonly IngredientFaker _ingredientFaker;
        private readonly UserFaker _userFaker;
        private readonly Faker _faker;

        public RecipeRepositoryTests()
        {
            _dbContext = new DatabaseContextFaker().Generate();
            _ingredientFaker = new IngredientFaker();
            _recipeFaker = new RecipeFaker();
            _userFaker = new UserFaker();
            _faker = new Faker();
        }

        [Fact]
        public async Task Should_get_by_ingredients_and_paginate_by_page_size_and_index_recipes_by_rating()
        {
            // Given
            var repository = GetRepository();
            var recipes = Enumerable.Range(1, 10).Select(x => _recipeFaker.Generate()).ToList();
            var ingredient = _ingredientFaker.Generate();

            recipes[0].AddIngredient(ingredient);
            recipes[1].AddIngredient(ingredient);
            recipes[2].AddIngredient(ingredient);

            _dbContext.AddRange(recipes);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetRecipesByIngredientsIdsAsync(new[] { ingredient.Id }, 2, 2);

            // Then
            result.Items.Should().ContainSingle();
        }

        [Fact]
        public async Task Should_get_recipes_by_ingredients()
        {
            // Given
            var repository = GetRepository();
            var recipe1 = _recipeFaker.Generate();
            var recipe2 = _recipeFaker.Generate();
            var ingredient = _ingredientFaker.Generate();

            recipe1.AddIngredient(ingredient);

            _dbContext.AddRange(recipe1, recipe2);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetRecipesByIngredientsIdsAsync(new[] { ingredient.Id });

            // Then
            result.Items.Should().ContainSingle();
            result.Items.First().Ingredients.First().Ingredient.Should().Be(ingredient);
        }

        [Fact]
        public async Task Should_get_recipe()
        {
            // Given
            var repository = GetRepository();
            var recipe = _recipeFaker.Generate();
            var ingredient = _ingredientFaker.Generate();
            var quantity = _faker.Random.Number(min: 1);
            var user = _userFaker.Generate();

            recipe.AddIngredient(ingredient);
            recipe.AddLike(user);

            _dbContext.AddRange(recipe);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetRecipe(recipe.Id);

            // Then
            result.Should().Be(recipe);
            result.Ingredients.Should().ContainSingle();
            result.Ingredients.First().Ingredient.Should().Be(ingredient);
            result.Likes.Should().ContainSingle();
            result.Likes.First().User.Should().Be(user);
        }

        [Fact]
        public async Task Should_get_ingredients()
        {
            // Given
            var repository = GetRepository();
            var ingredient1 = _ingredientFaker.Generate();
            var ingredient2 = _ingredientFaker.Generate();

            _dbContext.AddRange(ingredient1, ingredient2);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetIngredientsAsync();

            // Then
            result.Items.Should().HaveCount(2);
            result.Items.First().Should().Be(ingredient1);
            result.Items.Skip(1).First().Should().Be(ingredient2);
        }

        [Fact]
        public async Task Should_paginate_by_page_size_and_index_ingredients()
        {
            // Given
            var repository = GetRepository();
            var ingredients = Enumerable.Range(1, 10).Select(x => _ingredientFaker.Generate());

            _dbContext.AddRange(ingredients);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetIngredientsAsync(8, 2);

            // Then
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task Should_paginate_by_query_ingredients()
        {
            // Given
            var repository = GetRepository();
            var ingredient1 = _ingredientFaker.Generate();
            var ingredient2 = _ingredientFaker.Generate();

            _dbContext.AddRange(ingredient1, ingredient2);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetIngredientsAsync(query: ingredient1.Name);

            // Then
            result.Items.Should().ContainSingle();
            result.Items.First().Should().Be(ingredient1);
        }

        [Fact]
        public async Task Should_get_recipes_by_rating()
        {
            // Given
            var repository = GetRepository();
            var recipe1 = _recipeFaker.Generate();
            var recipe2 = _recipeFaker.Generate();

            recipe1.AddLike(_userFaker.Generate());

            _dbContext.AddRange(recipe1, recipe2);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetRecipesByRatingAsync();

            // Then
            result.Items.Should().HaveCount(2);
            result.Items.First().Should().Be(recipe1);
            result.Items.Skip(1).First().Should().Be(recipe2);
        }

        [Fact]
        public async Task Should_paginate_by_page_size_and_index_recipes_by_rating()
        {
            // Given
            var repository = GetRepository();
            var recipes = Enumerable.Range(1, 10).Select(x => _recipeFaker.Generate());

            _dbContext.AddRange(recipes);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetRecipesByRatingAsync(8, 2);

            // Then
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task Should_paginate_by_query_recipes_by_rating()
        {
            // Given
            var repository = GetRepository();
            var recipe1 = _recipeFaker.Generate();
            var recipe2 = _recipeFaker.Generate();

            _dbContext.AddRange(recipe1, recipe2);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetRecipesByRatingAsync(query: recipe1.Name);

            // Then
            result.Items.Should().ContainSingle();
            result.Items.First().Should().Be(recipe1);
        }

        [Fact]
        public async Task Should_return_false_when_ingredient_exists()
        {
            // Given
            var repository = GetRepository();
            var ingredient1 = _ingredientFaker.Generate();

            _dbContext.Add(ingredient1);
            _dbContext.SaveChanges();

            // When
            var result = await repository.AnyIngredientDoesNotExistsAsync(new[] { ingredient1.Id });

            // Then
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_return_true_when_ingredient_does_not_exists()
        {
            // Given
            var repository = GetRepository();

            // When
            var result = await repository.AnyIngredientDoesNotExistsAsync(new[] { Guid.NewGuid() });

            // Then
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_get_ingredients_from_ids()
        {
            // Given
            var repository = GetRepository();
            var ingredient1 = _ingredientFaker.Generate();
            var ingredient2 = _ingredientFaker.Generate();

            _dbContext.AddRange(ingredient1, ingredient2);
            _dbContext.SaveChanges();

            // When
            var result = await repository.GetIngredientsFromIdsAsync(new[] { ingredient1.Id, ingredient2.Id });

            // Then
            result.Should().NotBeEmpty();
            result.Should().Contain(ingredient1);
            result.Should().Contain(ingredient2);
        }

        public RecipeRepository GetRepository() => new(_dbContext);
    }
}