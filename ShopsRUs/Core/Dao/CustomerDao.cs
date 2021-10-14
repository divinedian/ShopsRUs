using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Dao
{
    public class CustomerDao : ICustomerDao
    {
        private readonly AppDbContext _context;
        public CustomerDao(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateCustomer(Customer customer, string userTypeTitle)
        {
            //confirm if a customer with same username already exists in database
            if (_context.Customers.Any(c => c.UserName == customer.UserName)) throw new Exception("Username already exists");

            //confirm that the usertype exists
            var userType = _context.UserTypes.FirstOrDefault(u => u.Title == userTypeTitle);
            if (userType == null) throw new Exception($"There is no usertype with title: {customer.UserType.Title}");

            customer.UserType = userType;
            customer.UserTypeId = string.Empty;
            customer.UserTypeId = userType.UserTypeId;
            customer.Orders = new List<Order>();

            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();

            return customer.CustomerId;
        }

        public Task<List<Customer>> GetAllCustomers()
        {
            return _context.Customers.Include(c => c.Orders)
                                        .ThenInclude(o => o.OrderDetails)
                                        .Include(c => c.UserType)
                                        .ThenInclude(u => u.Discount)
                                        .ToListAsync();
        }

        public Task<Customer> GetCustomerByUserName(string Username)
        {
            var customer = _context.Customers.Include(c => c.Orders)
                               .Include(c => c.UserType)
                               .ThenInclude(u => u.Discount)
                               .Where(c => c.UserName == Username)
                               .SingleOrDefaultAsync();

            if (customer.Result == null)
            {
                throw new Exception($"Customer with username:'{Username}' does not exist");
            }
            return customer;
        }

        public Task<Customer> GetCustomerById(string id)
        {
           var customer =  _context.Customers.Include(c => c.Orders)
                               .Include(c => c.UserType)
                               .ThenInclude(u => u.Discount)
                               .Where(c => c.CustomerId == id)
                               .SingleOrDefaultAsync();
            if (customer.Result == null)
            {
                throw new Exception($"Customer with id:'{id}'does not exist");
            }
            return customer;
        }

        public async Task<decimal> GetCustomersDiscount(string customerId)
        {
            var list = new List<decimal>();
            try
            {
                var customer = await GetCustomerById(customerId);
                var noOfYears = await CalculateYearsSinceRegistration(customer.DateRegistered);
                var discounts = await _context.Discounts.Where(d => d.DiscountType == customer.UserType.Title).ToListAsync();
                foreach(var discount in discounts)
                {
                    if (noOfYears >= discount.Duration) list.Add(discount.Percentage);
                }
                return list.Max();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<int> CalculateYearsSinceRegistration(DateTime dateRegister)
        {
            return Task.FromResult(Convert.ToInt32(Math.Floor(Convert.ToInt32((DateTime.Now - dateRegister).TotalDays) / 365.2)));
        }
    }
}
