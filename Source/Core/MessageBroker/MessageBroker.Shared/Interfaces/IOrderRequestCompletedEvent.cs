namespace MessageBroker.Shared.Interfaces
{
    public interface IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}