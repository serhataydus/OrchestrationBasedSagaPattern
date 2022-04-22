using MessageBroker.Shared.Models.Order;
using MessageBroker.Shared.Models.Payment;

namespace MessageBroker.Shared.Interfaces
{
    public interface IOrderCreatedRequestEvent
    {
        int OrderId { get; set; }
        string BuyerId { get; set; }
        PaymentMessage Payment { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
    }
}