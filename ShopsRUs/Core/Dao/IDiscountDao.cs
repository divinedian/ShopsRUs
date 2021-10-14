using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Dao
{
    public interface IDiscountDao
    {
        Task AddDiscount(Discount discount);

        Task<List<Discount>> GetAllDiscount();

        Task<decimal> GetDiscountPercentage(string discountType);

        Task<Discount> GetDiscount(string discount);
    }
}
