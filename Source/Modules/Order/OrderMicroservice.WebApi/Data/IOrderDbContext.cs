using Microsoft.EntityFrameworkCore;
using OrderMicroservice.WebApi.Data.Entities;

namespace OrderMicroservice.WebApi.Data
{
    public interface IOrderDbContext
    {
        DbSet<OrderEntity> Orders { get; set; }
        DbSet<OrderItemEntity> OrderItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
