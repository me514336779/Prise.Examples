using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersControllerPlugin.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIncVAT { get; set; }
    }
}
