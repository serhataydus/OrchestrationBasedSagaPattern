using Microsoft.AspNetCore.Mvc;

namespace PaymentMicroservice.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet(Name = "Get")]
    public bool Get()
    {
        return true;
    }
}
