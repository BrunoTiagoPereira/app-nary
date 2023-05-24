using AppNary.Domain.Recipes.Services;
using MediatR;

namespace AppNary.Domain.Recipes.Events.Handlers
{
    public class OnRecipeRemovedEventHandler : INotificationHandler<OnRecipeRemovedEvent>
    {
        private readonly IImageStorageManager _imageStorageManager;

        public OnRecipeRemovedEventHandler(IImageStorageManager imageStorageManager)
        {
            _imageStorageManager = imageStorageManager ?? throw new ArgumentNullException(nameof(imageStorageManager));
        }

        public async Task Handle(OnRecipeRemovedEvent notification, CancellationToken cancellationToken)
        {
            await _imageStorageManager.Remove(notification.AggregateRootId);
        }
    }
}