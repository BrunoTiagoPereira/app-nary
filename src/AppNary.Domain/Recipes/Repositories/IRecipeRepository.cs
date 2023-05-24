using AppNary.Core.Application.Pagination;
using AppNary.Domain.Recipes.Entities;
using ProductsPricing.Core.Data;

namespace AppNary.Domain.Recipes.Repositories
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<Recipe?> GetRecipe(Guid recipeId);
        Task<bool> AnyIngredientDoesNotExistsAsync(IEnumerable<Guid> ingredientsIds);
        Task<List<Ingredient>> GetIngredientsFromIdsAsync(IEnumerable<Guid> ingredientsIds);
        Task<PagedResult<Ingredient>> GetIngredientsAsync(int pageSize = PagedResult.DEFAULT_PAGE_SIZE, int pageIndex = PagedResult.DEFAULT_PAGE_INDEX, string query = PagedResult.DEFAULT_QUERY);
        Task<PagedResult<Recipe>> GetRecipesByRatingAsync(int pageSize = PagedResult.DEFAULT_PAGE_SIZE, int pageIndex = PagedResult.DEFAULT_PAGE_INDEX, string query = PagedResult.DEFAULT_QUERY, Guid? userId = null);
        Task<PagedResult<Recipe>> GetRecipesByIngredientsIdsAsync(IEnumerable<Guid> ingredientsIds, int pageSize = PagedResult.DEFAULT_PAGE_SIZE, int pageIndex = PagedResult.DEFAULT_PAGE_INDEX);
    }
}