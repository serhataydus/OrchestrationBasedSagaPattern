using Automatonymous;
using MassTransit;
using MessageBroker.Shared.Constants;
using MessageBroker.Shared.Events;
using MessageBroker.Shared.Interfaces;
using MessageBroker.Shared.Messages;
using MessageBroker.Shared.Models.Payment;
using StateMachine.WorkerService.Data.Enities;

namespace StateMachine.WorkerService.Services
{
    public class OrderStateMachineService : MassTransitStateMachine<OrderStateInstanceEntity>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; private set; }
        public Event<IStockReservedEvent> StockReservedEvent { get; private set; }
        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; private set; }
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; private set; }
        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; private set; }

        public State OrderCreated { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }

        public OrderStateMachineService()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Initially(
             When(OrderCreatedRequestEvent)
             .Then(context =>
             {
                 context.Instance.BuyerId = context.Data.BuyerId;

                 context.Instance.OrderId = context.Data.OrderId;
                 context.Instance.CreationDate = DateTime.UtcNow;

                 context.Instance.CardName = context.Data.Payment.CardName;
                 context.Instance.CardNumber = context.Data.Payment.CardNumber;
                 context.Instance.CVV = context.Data.Payment.CVV;
                 context.Instance.Expiration = context.Data.Payment.Expiration;
                 context.Instance.TotalAmount = context.Data.Payment.TotalAmount;
             })
            .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent before : {context.Instance}"); })
            .Publish(context => new OrderCreatedEvent(context.Instance.CorrelationId) { OrderItems = context.Data.OrderItems })
            .TransitionTo(OrderCreated)
            .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent After : {context.Instance}"); }));

            During(OrderCreated,
                When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{RabbitMqConstants.PaymentStockReservedRequestQueueName}"), context => new StockReservedRequestPayment(context.Instance.CorrelationId)
                {
                    OrderItems = context.Data.OrderItems,
                    Payment = new PaymentMessage()
                    {
                        CardName = context.Instance.CardName,
                        CardNumber = context.Instance.CardNumber,
                        CVV = context.Instance.CVV,
                        Expiration = context.Instance.Expiration,
                        TotalAmount = context.Instance.TotalAmount
                    },
                    BuyerId = context.Instance.BuyerId
                }).Then(context => { Console.WriteLine($"StockReservedEvent After : {context.Instance}"); }),
                When(StockNotReservedEvent).TransitionTo(StockNotReserved).Publish(context => new OrderRequestFailedEvent() { OrderId = context.Instance.OrderId, Reason = context.Data.Reason }).Then(context => { Console.WriteLine($"StockReservedEvent After : {context.Instance}"); })

                );

            During(StockReserved,
                When(PaymentCompletedEvent).TransitionTo(PaymentCompleted).Publish(context => new OrderRequestCompletedEvent() { OrderId = context.Instance.OrderId }).Then(context => { Console.WriteLine($"PaymentCompletedEvent After : {context.Instance}"); })
                .Finalize(),
                When(PaymentFailedEvent)
                .Publish(context => new OrderRequestFailedEvent() { OrderId = context.Instance.OrderId, Reason = context.Data.Reason })
                .Send(new Uri($"queue:{RabbitMqConstants.StockRollBackMessageQueueName}"), context => new StockRollbackMessage() { OrderItems = context.Data.OrderItems }).TransitionTo(PaymentFailed).Then(context => { Console.WriteLine($"PaymentFailedEvent After : {context.Instance}"); })

                );

            SetCompletedWhenFinalized();
        }
    }
}
