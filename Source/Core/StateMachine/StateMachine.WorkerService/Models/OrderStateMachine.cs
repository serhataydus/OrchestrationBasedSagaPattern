using MassTransit;
using MessageBroker.Shared.Interfaces;
using StateMachine.WorkerService.Data.Enities;

namespace StateMachine.WorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstanceEntity>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }

        public State OrderCreated { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Initially(When(OrderCreatedRequestEvent).Then(context =>
            {
                context.Instance.BuyerId = context.Data.BuyerId;

                context.Instance.OrderId = context.Data.OrderId;
                context.Instance.CreationDate = DateTime.Now;

                context.Instance.CardName = context.Data.Payment.CardName;
                context.Instance.CardNumber = context.Data.Payment.CardNumber;
                context.Instance.CVV = context.Data.Payment.CVV;
                context.Instance.Expiration = context.Data.Payment.Expiration;
                context.Instance.TotalAmount = context.Data.Payment.TotalAmount;
            }).Then(context => { Console.WriteLine($"OrderCreatedRequestEvent Before : {context.ToString()}"); }).TransitionTo(OrderCreated).Then(context => { Console.WriteLine($"OrderCreatedRequestEvent After : {context.Instance}"); }));
        }
    }
}
