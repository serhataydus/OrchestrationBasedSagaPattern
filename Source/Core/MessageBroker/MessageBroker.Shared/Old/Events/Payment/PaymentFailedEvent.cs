using MessageBroker.Shared.Messages.Order;

namespace MessageBroker.Shared.Events.Payment;

public class PaymentFailedEvent
{
    public int OrderId { get; set; }
    public string BuyerId { get; set; }
    public string FailMessage { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; }
}
