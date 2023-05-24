using MediatR;

namespace AppNary.Core.DomainObjects
{
    public abstract class Event : INotification
    {
        public Guid AggregateRootId { get; set; }

        public DateTime TimeStamp { get; set; }

        public Event(Guid aggregateRootId)
        {
            AggregateRootId = aggregateRootId;
            TimeStamp = DateTime.Now;
        }
    }
}