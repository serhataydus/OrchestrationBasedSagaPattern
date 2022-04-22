using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMicroservice.WebApi.Data;

namespace StockMicroservice.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly StockDbContext _stockDbContext;

    public StockController(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _stockDbContext.Stocks.ToListAsync());
    }
}
