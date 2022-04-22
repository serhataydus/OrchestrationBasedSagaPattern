using MassTransit;
using MessageBroker.Shared.Constants;
using MessageBroker.Shared.Events;
using MessageBroker.Shared.Interfaces;
using MessageBroker.Shared.Models.Order;
using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.WebApi.Data;
using OrderMicroservice.WebApi.Data.Entities;
using OrderMicroservice.WebApi.Enums;
using OrderMicroservice.WebApi.Models.Dtos.Order;

namespace OrderMicroservice.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public OrderController(OrderDbContext orderDbContext, ISendEndpointProvider sendEndpointProvider)
    {
        _orderDbContext = orderDbContext;
        _sendEndpointProvider = sendEndpointProvider;
    }

    [HttpPost]
    public async Task<IActionResult> Get(OrderCreateDto orderCreate, CancellationToken cancellationToken)
    {
        OrderEntity newOrder = new()
        {
            BuyerId = orderCreate.BuyerId,
            Status = OrderStatus.Suspend,
            Address = new()
            {
                District = orderCreate.Address.District,
                Line = orderCreate.Address.Line,
                Province = orderCreate.Address.Province
            },
            CreationDate = DateTime.UtcNow,
            Items = new List<OrderItemEntity>()
        };

        orderCreate.OrderItems.ForEach(item =>
        {
            newOrder.Items.Add(new()
            {
                Price = item.Price,
                ProductId = item.ProductId,
                Count = item.Count
            });
        });

        await _orderDbContext.Orders.AddAsync(newOrder, cancellationToken);
        await _orderDbContext.SaveChangesAsync(cancellationToken);

        OrderCreatedRequestEvent orderCreatedRequestEvent = new()
        {
            BuyerId = orderCreate.BuyerId,
            OrderId = newOrder.Id,
            Payment = new()
            {
                CardName = orderCreate.Payment.CardName,
                CardNumber = orderCreate.Payment.CardNumber,
                CVV = orderCreate.Payment.CVV,
                Expiration = orderCreate.Payment.Expiration,
                TotalAmount = orderCreate.OrderItems.Sum(s => s.Price * s.Count)
            },
            OrderItems = orderCreate.OrderItems.Select(s => new OrderItemMessage { Count = s.Count, ProductId = s.ProductId }).ToList()
        };

        ISendEndpoint? sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMqConstant.OrderSagaQueueName}"));
        await sendEndpoint.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent, cancellationToken);

        return Ok();
    }
}
