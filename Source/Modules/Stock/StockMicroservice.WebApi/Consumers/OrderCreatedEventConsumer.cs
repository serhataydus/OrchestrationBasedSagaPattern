using MassTransit;
using MessageBroker.Shared.Events;
using MessageBroker.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using StockMicroservice.WebApi.Data;

namespace StockMicroservice.WebApi.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        private readonly StockDbContext _stockDbContext;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(StockDbContext stockDbContext, ILogger<OrderCreatedEventConsumer> logger, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _stockDbContext = stockDbContext;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            List<bool>? stockResult = new List<bool>();

            foreach (MessageBroker.Shared.Models.Order.OrderItemMessage? item in context.Message.OrderItems)
            {
                stockResult.Add(await _stockDbContext.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
            }

            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (MessageBroker.Shared.Models.Order.OrderItemMessage? item in context.Message.OrderItems)
                {
                    Data.Entities.StockEntity? stock = await _stockDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }

                    await _stockDbContext.SaveChangesAsync();
                }

                _logger.LogInformation($"Stock was reserved for CorrelationId Id :{context.Message.CorrelationId}");

                StockReservedEvent stockReservedEvent = new StockReservedEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems
                };

                await _publishEndpoint.Publish(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent(context.Message.CorrelationId)
                {
                    Reason = "Not enough stock"
                });

                _logger.LogInformation($"Not enough stock for CorrelationId Id :{context.Message.CorrelationId}");
            }
        }
    }
}