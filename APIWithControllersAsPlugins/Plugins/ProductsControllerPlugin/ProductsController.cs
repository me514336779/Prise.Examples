using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsControllerPlugin.Models;

namespace ProductsControllerPlugin
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        public ProductsController()
        {
        }

        public ControllerBase AsControllerBase() => this as ControllerBase;

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return new List<Product> {
                new Product
                {
                    Id = 1,
                    Name = "Test"
                }
            };
        }
    }
}
