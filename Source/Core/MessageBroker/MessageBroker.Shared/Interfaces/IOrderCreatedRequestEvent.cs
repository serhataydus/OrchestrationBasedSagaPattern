using MessageBroker.Shared.Models.Order;
using MessageBroker.Shared.Models.Payment;

namespace MessageBroker.Shared.Interfaces
{
    public interface IOrderCreatedRequestEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }

        public PaymentMessage Payment { get; set; }
    }
}