using MassTransit;
using MessageBroker.Shared.Models.Order;

namespace MessageBroker.Shared.Interfaces
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        List<OrderItemMessage> OrderItems { get; set; }
    }
}