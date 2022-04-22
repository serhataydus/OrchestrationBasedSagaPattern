using OrderMicroservice.WebApi.Models.Dtos.Address;
using OrderMicroservice.WebApi.Models.Dtos.Payment;

namespace OrderMicroservice.WebApi.Models.Dtos.Order
{
    public class OrderCreateDto
    {
        public string BuyerId { get; set; }
        public PaymentDto Payment { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
        public AddressDto Address { get; set; }
    }
}
