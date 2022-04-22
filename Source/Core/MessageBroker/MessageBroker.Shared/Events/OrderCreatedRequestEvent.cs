using MessageBroker.Shared.Interfaces;
using MessageBroker.Shared.Models.Order;
using MessageBroker.Shared.Models.Payment;

namespace MessageBroker.Shared.Events
{
    public class OrderCreatedRequestEvent : IOrderCreatedRequestEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new();
    }
}
