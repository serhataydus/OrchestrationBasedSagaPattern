using MassTransit;
using MessageBroker.Shared.Models.Order;

namespace MessageBroker.Shared.Interfaces
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}