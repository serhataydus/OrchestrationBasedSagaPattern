using MassTransit;

namespace StateMachine.WorkerService.Data.Enities
{
    public class OrderStateInstanceEntity : SagaStateMachineInstance 
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string BuyerId { get; set; }
        public int OrderId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
