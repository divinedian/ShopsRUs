using System;
using System.Collections.Generic;

namespace ShopsRUs.Data.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderPlaced { get; set; }
    }
}