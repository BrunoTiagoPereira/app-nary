using AppNary.Core.DomainObjects;

namespace AppNary.Domain.Recipes.Events
{
    public class OnRecipeRemovedEvent : Event
    {
        public OnRecipeRemovedEvent(Guid aggregateRootId) : base(aggregateRootId)
        {
        }
    }
}