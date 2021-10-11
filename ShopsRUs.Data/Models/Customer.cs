using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Data.Models
{
    public class Customer
    {
        public string CustomerId { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Order> Orders { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.Now;
        public string UserTypeId { get; set; }
        public UserType UserType { get; set; }
    }
}
