using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Core.Core.Application.Queries;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Dao
{
    public interface IInvoiceDao
    {
        Task CreateCategory(Category category);

        Task CreateItem(Item item);

        Task MakeAnOrder(Order order);
        Task<(List<OrderDetail>,List<OrderDetail>,Order)> AddOrderDetailsToOrder(Order order, List<OrderItem> items);

        Task<Order> GetOrderGivenOrderId(string orderId);

        Task<decimal> GetInvoiceAmountFromBill(string orderId);
        InvoiceDisplayed PrepareBill(List<OrderDetail> orderforpercentdiscount,
            List<OrderDetail> noPercentDiscount, decimal percentage, Order order);
    }
}
