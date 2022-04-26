using MessageBroker.Shared.Models.Order;

namespace MessageBroker.Shared.Messages
{
    public class StockRollbackMessage : IStockRollBackMessage
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}