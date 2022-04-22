namespace OrderMicroservice.WebApi.Models.Dtos.Order
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
