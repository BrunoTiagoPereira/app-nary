using AppNary.Domain.Recipes.Events;
using AppNary.Domain.Recipes.Events.Handlers;
using AppNary.Domain.Recipes.Services;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppNary.Domain.UnitTest.Recipes.Events.Handlers
{
    public class OnRecipeRemovedEventHandlerTests
    {
        private readonly Mock<IImageStorageManager> _imageStorageManager;

        public OnRecipeRemovedEventHandlerTests()
        {
            _imageStorageManager = new Mock<IImageStorageManager>();
        }

        [Fact]
        public void Should_throw_when_creating_with_null_image_storage_manager()
        {
            FluentActions.Invoking(() => new OnRecipeRemovedEventHandler(default(IImageStorageManager))).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_handle()
        {
            // Given
            var handler = new OnRecipeRemovedEventHandler(_imageStorageManager.Object);
            var recipeId = Guid.NewGuid();

            // When
            await handler.Handle(new OnRecipeRemovedEvent(recipeId), CancellationToken.None);

            // Then
            _imageStorageManager.Verify(x => x.Remove(recipeId));
        }
    }
}