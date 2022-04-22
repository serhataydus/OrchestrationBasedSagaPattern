using Microsoft.EntityFrameworkCore;
using OrderMicroservice.WebApi.Data.Entities;

namespace OrderMicroservice.WebApi.Data;

public class OrderDbContext : DbContext, IOrderDbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
}
