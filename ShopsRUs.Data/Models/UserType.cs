using System;

namespace ShopsRUs.Data.Models
{
    public class UserType
    {
        public string UserTypeId { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}