using Microsoft.AspNetCore.Http;

namespace AppNary.Domain.Recipes.Services
{
    public interface IImageStorageManager
    {
        Task<string> Save(Guid recipeId, IFormFile formFile);
        Task Remove(Guid recipeId);
    }
}