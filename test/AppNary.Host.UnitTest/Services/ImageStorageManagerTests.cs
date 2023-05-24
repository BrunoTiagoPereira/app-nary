using AppNary.Host.Services;
using AppNary.UnitTest.Abstractions.Fakes;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppNary.Host.UnitTest.Services
{
    public class ImageStorageManagerTests
    {
        private readonly Mock<BlobContainerClient> _blobContainerClientMock;
        private readonly Mock<BlobClient> _blobClientMock;
        private readonly Mock<IFormFile> _formFileMock;
        private readonly Faker _faker;

        public ImageStorageManagerTests()
        {
            _blobContainerClientMock = new Mock<BlobContainerClient>();
            _blobClientMock = new Mock<BlobClient>();
            _formFileMock = new Mock<IFormFile>();
            _faker = new Faker();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_client()
        {
            FluentActions.Invoking(() => new ImageStorageManager(default(BlobContainerClient))).Should().Throw<ArgumentNullException>();    
        }

        [Fact]
        public async Task Should_remove()
        {
            var manager = GetManager();
            var recipeId = Guid.NewGuid();
            var blobName = $"{recipeId}.png";

            _blobContainerClientMock.Setup(x => x.GetBlobClient(blobName)).Returns(_blobClientMock.Object);

            await manager.Remove(recipeId);

            _blobContainerClientMock.Verify(x => x.GetBlobClient(blobName));
            _blobClientMock.Verify(x => x.DeleteAsync(It.IsAny<DeleteSnapshotsOption>(), It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Should_save_when_blob_exists()
        {
            var manager = GetManager();
            var recipeId = Guid.NewGuid();
            var fileStream = GetTempFileStream();
            var blobUrl = _faker.Internet.Url();
            var blobName = $"{recipeId}.png";

            _formFileMock.Setup(x => x.FileName).Returns(fileStream.Name);
            _formFileMock.Setup(x => x.OpenReadStream()).Returns(fileStream);
            _blobClientMock.Setup(x => x.Uri).Returns(new Uri(blobUrl));
            _blobClientMock.Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Response.FromValue(true, new AzureResponseFaker()));
            _blobContainerClientMock.Setup(x => x.GetBlobClient(blobName)).Returns(_blobClientMock.Object);

            var result = await manager.Save(recipeId, _formFileMock.Object);

            _blobContainerClientMock.Verify(x => x.GetBlobClient(blobName));
            _blobClientMock.Verify(x => x.ExistsAsync(It.IsAny<CancellationToken>()));
            _blobClientMock.Verify(x => x.DeleteAsync(It.IsAny<DeleteSnapshotsOption>(), It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()));
            _blobClientMock.Verify(x => x.UploadAsync(fileStream));
        }

        [Fact]
        public async Task Should_save_when_blob_does_not_exists()
        {
            var manager = GetManager();
            var recipeId = Guid.NewGuid();
            var fileStream = GetTempFileStream();
            var blobUrl = _faker.Internet.Url();
            var blobName = $"{recipeId}.png";

            _formFileMock.Setup(x => x.FileName).Returns(fileStream.Name);
            _formFileMock.Setup(x => x.OpenReadStream()).Returns(fileStream);
            _blobClientMock.Setup(x => x.Uri).Returns(new Uri(blobUrl));
            _blobClientMock.Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Response.FromValue(false, new AzureResponseFaker()));
            _blobContainerClientMock.Setup(x => x.GetBlobClient(blobName)).Returns(_blobClientMock.Object);

            var result = await manager.Save(recipeId, _formFileMock.Object);

            _blobContainerClientMock.Verify(x => x.GetBlobClient(blobName));
            _blobClientMock.Verify(x => x.ExistsAsync(It.IsAny<CancellationToken>()));
            _blobClientMock.Verify(x => x.DeleteAsync(It.IsAny<DeleteSnapshotsOption>(), It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()), Times.Never());
            _blobClientMock.Verify(x => x.UploadAsync(fileStream));
        }

        private ImageStorageManager GetManager() => new(_blobContainerClientMock.Object);

        private static FileStream GetTempFileStream()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.png");
            File.Create(filePath).Dispose();

            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        }
    }
}
