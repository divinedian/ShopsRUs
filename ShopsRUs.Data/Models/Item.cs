using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Data.Models
{
    public class Item
    {
        public string ItemId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsItemOfTheWeek { get; set; } = true;
        public bool InStock { get; set; } = true;
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
