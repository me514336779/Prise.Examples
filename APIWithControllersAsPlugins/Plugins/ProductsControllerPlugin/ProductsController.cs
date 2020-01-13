using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prise.Plugin;
using ProductsControllerPlugin.Models;

namespace ProductsControllerPlugin
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsDbContext dbContext;
        internal ProductsController(ProductsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [PluginFactory]
        public static ProductsController CreateInstanceOfController(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(ProductsDbContext));
            return new ProductsController(service as ProductsDbContext);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await dbContext.Products
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
