using Microsoft.EntityFrameworkCore;

namespace OrderMicroservice.WebApi.Data.Entities
{
    [Owned]
    public class AddressEntity
    {
        public string Line { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
