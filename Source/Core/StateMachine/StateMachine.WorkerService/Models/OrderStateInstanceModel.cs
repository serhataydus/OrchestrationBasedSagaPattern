using MassTransit;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace StateMachine.WorkerService.Models
{
    public class OrderStateInstanceModel : SagaStateMachineInstance
    {
        private readonly ObjectPool<StringBuilder> _builderPool;

        public OrderStateInstanceModel(ObjectPool<StringBuilder> builderPool)
        {
            _builderPool = builderPool;
        }

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

        public override string ToString()
        {
            StringBuilder stringBuilder = _builderPool.Get();
            try
            {
                GetType().GetProperties().ToList().ForEach(p =>
                {
                    object? value = p.GetValue(this, null);
                    stringBuilder.Append($"{p.Name} : {value}");
                });

                stringBuilder.Append("----------------------------------");

                return stringBuilder.ToString();
            }
            finally
            {
                _builderPool.Return(stringBuilder);
            }
        }
    }
}
