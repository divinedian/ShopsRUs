using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Dao
{
    public interface ICustomerDao
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(string id);
        Task<Customer> GetCustomerByUserName(string Username);
        Task<string> CreateCustomer(Customer customer, string userTypeTitle);
        Task<decimal> GetCustomersDiscount(string customerId);
        Task<int> CalculateYearsSinceRegistration(DateTime dateRegister);
    }
}
