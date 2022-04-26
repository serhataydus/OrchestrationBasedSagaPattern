using MassTransit;
using MessageBroker.Shared.Models.Order;
using MessageBroker.Shared.Models.Payment;

namespace MessageBroker.Shared.Interfaces
{
    public interface IStockReservedRequestPayment : CorrelatedBy<Guid>
    {
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }

        public string BuyerId { get; set; }
    }
}