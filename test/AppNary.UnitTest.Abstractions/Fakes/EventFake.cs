using AppNary.Core.DomainObjects;

namespace AppNary.UnitTest.Abstractions.Fakes
{
    public class EventFake : Event
    {
        public EventFake(Guid aggregateRootId) : base(aggregateRootId)
        {
        }
    }
}