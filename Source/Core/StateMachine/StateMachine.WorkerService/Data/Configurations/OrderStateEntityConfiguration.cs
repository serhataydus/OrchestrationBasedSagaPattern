using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StateMachine.WorkerService.Data.Enities;

namespace StateMachine.WorkerService.Data.Configurations
{
    internal class OrderStateEntityConfiguration : SagaClassMap<OrderStateInstanceEntity>
    {
        protected override void Configure(EntityTypeBuilder<OrderStateInstanceEntity> entity, ModelBuilder model)
        {
            entity.Property(x => x.BuyerId).HasMaxLength(256);
        }
    }
}
