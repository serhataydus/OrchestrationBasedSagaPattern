using OrderMicroservice.WebApi.Enums;

namespace OrderMicroservice.WebApi.Data.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string BuyerId { get; set; }
        public OrderStatus Status { get; set; }
        public string? FailMessage { get; set; }
        public AddressEntity Address { get; set; }

        public virtual ICollection<OrderItemEntity> Items { get; set; }
    }
}
