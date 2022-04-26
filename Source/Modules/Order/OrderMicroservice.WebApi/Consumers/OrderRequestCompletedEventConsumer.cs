using MassTransit;
using MessageBroker.Shared.Interfaces;
using OrderMicroservice.WebApi.Data;
using OrderMicroservice.WebApi.Enums;

namespace OrderMicroservice.WebApi.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        private readonly ILogger<OrderRequestCompletedEventConsumer> _logger;

        public OrderRequestCompletedEventConsumer(OrderDbContext orderDbContext, ILogger<OrderRequestCompletedEventConsumer> logger)
        {
            _orderDbContext = orderDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            Data.Entities.OrderEntity? order = await _orderDbContext.Orders.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = OrderStatus.Complete;
                await _orderDbContext.SaveChangesAsync();

                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
            }
            else
            {
                _logger.LogError($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}