using Microsoft.EntityFrameworkCore;
using StockMicroservice.WebApi.Data.Entities;

namespace StockMicroservice.WebApi.Data
{
    public interface IStockDbContext
    {
        DbSet<StockEntity> Stocks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
