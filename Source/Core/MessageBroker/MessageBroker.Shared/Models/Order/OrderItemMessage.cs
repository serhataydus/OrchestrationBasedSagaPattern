namespace MessageBroker.Shared.Models.Order
{
    public class OrderItemMessage
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
