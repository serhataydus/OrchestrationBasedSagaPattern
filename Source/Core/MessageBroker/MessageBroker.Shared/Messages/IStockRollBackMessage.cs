using MessageBroker.Shared.Models.Order;

namespace MessageBroker.Shared.Messages
{
    public interface IStockRollBackMessage
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}