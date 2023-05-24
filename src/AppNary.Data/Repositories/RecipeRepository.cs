using AppNary.Core.Application.Pagination;
using AppNary.Core.Data.Repositories;
using AppNary.Domain.Recipes.Entities;
using AppNary.Domain.Recipes.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppNary.Data.Repositories
{
    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(DbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Recipe>> GetRecipesByIngredientsIdsAsync(IEnumerable<Guid> ingredientsIds, int pageSize = PagedResult.DEFAULT_PAGE_SIZE, int pageIndex = PagedResult.DEFAULT_PAGE_INDEX)
        {
            var items = await Set
                .Include(x => x.Likes)
                .Where(x => x.Ingredients.Select(y => y.IngredientId).Any(y => ingredientsIds.Contains(y)))
                .OrderByDescending(x => x.Likes.Count())
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync()
                ;

            return new PagedResult<Recipe>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = string.Empty,
                TotalResults = items.Count
            };
        }

        public async Task<PagedResult<Ingredient>> GetIngredientsAsync(int pageSize = PagedResult.DEFAULT_PAGE_SIZE, int pageIndex = PagedResult.DEFAULT_PAGE_INDEX, string query = PagedResult.DEFAULT_QUERY)
        {
            var items = await _context
                .Set<Ingredient>()
                .Where(x => EF.Functions.Like(x.Name, $"%{query}%"))
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Ingredient>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query,
                TotalResults = items.Count
            };
        }

        public async Task<PagedResult<Recipe>> GetRecipesByRatingAsync(int pageSize = PagedResult.DEFAULT_PAGE_SIZE, int pageIndex = PagedResult.DEFAULT_PAGE_INDEX, string query = PagedResult.DEFAULT_QUERY, Guid? userId = null)
        {
            var items = await Set
                .Include(x => x.Likes)
                .Where(x => EF.Functions.Like(x.Name, $"%{query}%"))
                .Where(x => userId.HasValue ? x.UserId == userId.Value : true)
                .OrderByDescending(x => x.Likes.Count())
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Recipe>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query,
                TotalResults = items.Count
            };
        }

        public Task<bool> AnyIngredientDoesNotExistsAsync(IEnumerable<Guid> ingredientsIds)
        {
            return Task.FromResult(!(_context.Set<Ingredient>().Where(x => ingredientsIds.Contains(x.Id)).Count() == ingredientsIds.Count()));
        }

        public Task<List<Ingredient>> GetIngredientsFromIdsAsync(IEnumerable<Guid> ingredientsIds)
        {
            return _context.Set<Ingredient>().Where(x => ingredientsIds.Contains(x.Id)).ToListAsync();
        }

        public Task<Recipe?> GetRecipe(Guid recipeId)
        {
            return Set
                .Include(x => x.Likes)
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .Where(x => x.Id == recipeId)
                .FirstOrDefaultAsync()
                ;
        }
    }
}