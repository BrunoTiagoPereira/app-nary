using AppNary.Domain.Recipes.Services;
using Azure.Storage.Blobs;

namespace AppNary.Host.Services
{
    public class ImageStorageManager : IImageStorageManager
    {
        private readonly BlobContainerClient _blobContainerClient;

        public ImageStorageManager(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient ?? throw new ArgumentNullException(nameof(blobContainerClient));
        }

        public async Task<string> Save(Guid recipeId, IFormFile formFile)
        {
            var blobName = GetBlobName(recipeId);
            var blob = _blobContainerClient.GetBlobClient(blobName);

            if (await blob.ExistsAsync())
            {
                await blob.DeleteAsync();
            }

            return await InternalSave(blob, formFile);
        }

        private static string GetBlobName(Guid recipeId)
        {
            return $"{recipeId}.png";
        }

        private static async Task<string> InternalSave(BlobClient blob, IFormFile formFile)
        {
            using var fileStream = formFile.OpenReadStream();

            await blob.UploadAsync(fileStream);

            var blobUrl = blob.Uri.AbsoluteUri;

            return blobUrl;
        }

        public async Task Remove(Guid recipeId)
        {
            var blobName = GetBlobName(recipeId);
            var blob = _blobContainerClient.GetBlobClient(blobName);

            await blob.DeleteAsync();
        }
    }
}