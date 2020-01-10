using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc;
using OrdersControllerPlugin.Models;

namespace OrdersControllerPlugin
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly TableStorageProvider<OrderTableEntity> tableStorageProvider;

        public OrdersController(IFeatureServiceProvider featureServiceProvider)
        {
            this.tableStorageProvider = featureServiceProvider.GetService<TableStorageProvider<OrderTableEntity>>();
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            await this.tableStorageProvider.ConnectToTableAsync();
            var tableEntities = await this.tableStorageProvider.GetAll();
            return tableEntities.Select(t => ToOrder(t));
        }

        private Order ToOrder(OrderTableEntity e) => new Order
        {
            Id = e.Id,
            Price = e.Price,
            PriceIncVAT = e.PriceIncVAT
        };
    }
}
