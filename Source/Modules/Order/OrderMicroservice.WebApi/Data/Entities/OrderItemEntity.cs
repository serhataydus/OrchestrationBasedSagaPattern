namespace OrderMicroservice.WebApi.Data.Entities
{
    public class OrderItemEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }

        public virtual OrderEntity Order { get; set; }
    }
}
