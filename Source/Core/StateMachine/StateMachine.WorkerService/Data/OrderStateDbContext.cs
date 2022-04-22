using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using StateMachine.WorkerService.Data.Configurations;

namespace StateMachine.WorkerService.Data
{
    public class OrderStateDbContext : SagaDbContext, IOrderStateDbContext
    {
        public OrderStateDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateEntityConfiguration(); }
        }
    }
}
