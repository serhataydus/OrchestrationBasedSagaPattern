namespace MessageBroker.Shared.Events.Stock;

public class StockNotReservedEvent
{
    public int OrderId { get; set; }
    public string FailMessage { get; set; }
}
