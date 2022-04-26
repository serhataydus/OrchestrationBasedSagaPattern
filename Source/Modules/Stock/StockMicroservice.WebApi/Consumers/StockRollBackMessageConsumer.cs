using MassTransit;
using MessageBroker.Shared.Messages;
using Microsoft.EntityFrameworkCore;
using StockMicroservice.WebApi.Data;

namespace StockMicroservice.WebApi.Consumers
{
    public class StockRollBackMessageConsumer : IConsumer<IStockRollBackMessage>
    {
        private readonly StockDbContext _stockDbContext;
        private readonly ILogger<StockRollBackMessageConsumer> _logger;

        public StockRollBackMessageConsumer(StockDbContext stockDbContext, ILogger<StockRollBackMessageConsumer> logger)
        {
            _stockDbContext = stockDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IStockRollBackMessage> context)
        {
            foreach (MessageBroker.Shared.Models.Order.OrderItemMessage? item in context.Message.OrderItems)
            {
                Data.Entities.StockEntity? stock = await _stockDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _stockDbContext.SaveChangesAsync();
                }
            }

            _logger.LogInformation($"Stock was released");
        }
    }
}