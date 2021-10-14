using System;
using System.Collections.Generic;

namespace ShopsRUs.Data.Models
{
    public class Order
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderPlaced { get; set; } = DateTime.Now;
    }
}