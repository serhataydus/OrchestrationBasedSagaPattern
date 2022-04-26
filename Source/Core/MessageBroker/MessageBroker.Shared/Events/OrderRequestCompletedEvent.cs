using MessageBroker.Shared.Interfaces;

namespace MessageBroker.Shared.Events
{
    public class OrderRequestCompletedEvent : IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}