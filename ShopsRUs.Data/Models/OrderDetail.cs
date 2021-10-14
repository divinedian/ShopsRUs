using System;

namespace ShopsRUs.Data.Models
{
    public class OrderDetail
    {
        public string OrderDetailId { get; set; } = Guid.NewGuid().ToString();
        public string OrderId { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Item Item { get; set; }
        public Order Order { get; set; }
    }
}