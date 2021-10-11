
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Data.Models
{
    public class Discount
    {
        public string DiscountId { get; set; } = Guid.NewGuid().ToString();
        public string DiscountType { get; set; }
        public decimal Percentage { get; set; }
        public UserType UserType { get; set; }
    }
}
