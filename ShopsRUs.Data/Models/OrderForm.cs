using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Data.Models
{
    public class OrderForm
    {
        public string CustomerId { get; set; }
        public PlacedOrder placedOrder { get; set; }
    }
}
