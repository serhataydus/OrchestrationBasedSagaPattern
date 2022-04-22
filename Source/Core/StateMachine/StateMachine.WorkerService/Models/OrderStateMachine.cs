using MassTransit;
using StateMachine.WorkerService.Data.Enities;

namespace StateMachine.WorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstanceEntity>
    {
        protected OrderStateMachine()
        {
        }
    }
}
