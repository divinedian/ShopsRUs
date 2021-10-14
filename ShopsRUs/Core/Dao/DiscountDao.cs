using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Dao
{
    public class DiscountDao : IDiscountDao
    {
        private readonly AppDbContext _context;
        public DiscountDao(AppDbContext context)
        {
            _context = context;
        }

        public Task AddDiscount(Discount discount)
        {
            if (_context.Discounts.Any(d => d.DiscountType == discount.DiscountType && d.Duration == discount.Duration))
            {
                throw new Exception("Discount type name already exists for this customer, please rename or update discount");
            }

            if(!_context.UserTypes.Any(u=>u.Title == discount.DiscountType))
            {
                discount.UserType = new UserType();
                discount.UserType.Title = discount.DiscountType;
            }            

            _context.Discounts.Add(discount);
            return _context.SaveChangesAsync();
        }

        public Task<List<Discount>> GetAllDiscount()
        {
            return _context.Discounts.Include(d => d.UserType)
                                .ToListAsync();
        }

        public Task<Discount> GetDiscount(string discount)
        {
            var resp =  _context.Discounts.Include(d => d.UserType)
                                        .Where(d => d.DiscountType == discount)
                                        .SingleOrDefaultAsync();
            if (resp.Result == null) throw new Exception($"No discount type named: {discount}");
            return resp;
        }

        public Task<decimal> GetDiscountPercentage(string discountType)
        {
            try
            {
                var res = GetDiscount(discountType).Result;
                return Task.FromResult(res.Percentage);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
