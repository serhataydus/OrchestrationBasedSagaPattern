using Microsoft.EntityFrameworkCore;
using StockMicroservice.WebApi.Data.Entities;

namespace StockMicroservice.WebApi.Data;

public class StockDbContext : DbContext, IStockDbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<StockEntity> Stocks { get; set; }
}
