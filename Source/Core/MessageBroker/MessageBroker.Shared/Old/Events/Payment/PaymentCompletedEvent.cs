namespace MessageBroker.Shared.Events.Payment;

public class PaymentCompletedEvent
{
    public int OrderId { get; set; }
    public string BuyerId { get; set; }

}
