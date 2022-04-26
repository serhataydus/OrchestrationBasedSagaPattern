using MassTransit;
using MessageBroker.Shared.Models.Order;

namespace MessageBroker.Shared.Interfaces
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        List<OrderItemMessage> OrderItems { get; set; }
    }
}