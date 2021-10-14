using Microsoft.EntityFrameworkCore;
using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Dao
{
    public class InvoiceDao : IInvoiceDao
    {
        private readonly AppDbContext _context;

        public InvoiceDao(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<OrderDetail>, List<OrderDetail>, Order)> AddOrderDetailsToOrder(Order order, List<OrderItem> items)
        {
            var forPercentdiscount = new List<OrderDetail>();
            var NoPercentdiscount = new List<OrderDetail>();
            foreach (var OrderItem in items)
            {
                try
                {
                    var item = GetItemFromName(OrderItem.Item);
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ItemId = item.ItemId,
                        Price = item.Price,
                        Quantity = OrderItem.Quantities
                    };
                    order.OrderDetails.Add(orderDetail);
                    if (item.Category.CategoryName == "Grocery") NoPercentdiscount.Add(orderDetail);
                    else forPercentdiscount.Add(orderDetail);
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            await MakeAnOrder(order);
            return (forPercentdiscount, NoPercentdiscount,order);
        }

        public Item GetItemFromName(string title)
        {
            var item = _context.Items.Include(i=>i.Category).SingleOrDefault(i => i.Name == title);
            if (item == null) throw new Exception($"{title} does not exist");
            if (!item.InStock) throw new Exception($"{title} is out of stock");
            return item;
        }

        public InvoiceDisplayed PrepareBill(List<OrderDetail> orderforpercentdiscount, 
            List<OrderDetail> noPercentDiscount, decimal percentage, Order order)
        {
            
            var sumForPercentDiscount = 0M;
            var sumForNonPercentDiscount = 0M;
            foreach (var percent in orderforpercentdiscount)
            {
                sumForPercentDiscount += (percent.Quantity * percent.Price);
            }
            foreach(var nopercent in noPercentDiscount)
            {
                sumForNonPercentDiscount += (nopercent.Price * nopercent.Quantity);
            }
            var total = sumForPercentDiscount + sumForNonPercentDiscount;
            var DiscountByPercent = sumForPercentDiscount * percentage;
            var DiscountByNonPercent = Math.Floor(total / 100) * 5;

            return new InvoiceDisplayed
            {
                OrderPrice = total,
                NonPercentageDiscount = DiscountByNonPercent,
                PercentageDiscount = DiscountByPercent,
                PayableTotal = total-(DiscountByPercent+DiscountByNonPercent),
                order = order
            };            
        }

        public Task CreateCategory(Category category)
        {
            if (_context.Categories.Any(c => c.CategoryName == category.CategoryName)) throw new Exception("Category already exists");
            _context.Categories.Add(category);
            return _context.SaveChangesAsync();
        }

        public Task CreateItem(Item item)
        {
            if (!_context.Categories.Any(c => c.CategoryId == item.CategoryId)) throw new Exception($"Category with Id:{item.CategoryId} does not exist");
            if (!_context.Items.Any(i => i.Name == item.Name && i.CategoryId == item.CategoryId)) throw new Exception($"Category already has {item.Name}.");
            if (!_context.Items.Any(i => i.Name == item.Name)) throw new Exception($"{item.Name} already exists in this system.");
            _context.Items.Add(item);
            return _context.SaveChangesAsync();
        }

        public Task<decimal> GetInvoiceAmountFromBill(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderGivenOrderId(string orderId)
        {
            var order = _context.Orders.Include(o => o.Customer)
                           .ThenInclude(c => c.UserType)
                           .ThenInclude(u => u.Discount)
                           .Include(o => o.OrderDetails)
                           .ThenInclude(o => o.Item)
                           .ThenInclude(i => i.Category)
                           .Where(o => o.OrderId == orderId)
                           .SingleOrDefaultAsync();
            if (order.Result == null) throw new Exception($"order:'{orderId}' does not exist");
            return order;
        }        

        public Task MakeAnOrder(Order order)
        {
            _context.Orders.Add(order);
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Given an order id, we can get an already existing order using `GetOrderGivenOrderId` 
        /// and then calculate the discounted price using this method
        /// </summary>
        /// <param name="order"></param>
        /// <param name="discountDao"></param>
        /// <param name="customerDao"></param>
        /// <returns></returns>
        public async Task<InvoiceDisplayed> CalculatePercentageDiscountFromOrder(Order order, IDiscountDao discountDao, ICustomerDao customerDao)
        {
            decimal forPercentageDiscount = 0;
            decimal NoPercentageDiscount = 0;
            decimal percentage = 0;
            var discount = await discountDao.GetDiscount(order.Customer.UserType.Title);

            if (await customerDao.CalculateYearsSinceRegistration(order.Customer.DateRegistered) >= discount.Duration)
            {
                percentage = discount.Percentage;
            }
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.Item.Category.CategoryName == "Grocery")
                {
                    NoPercentageDiscount += (orderDetail.Price * orderDetail.Quantity);
                }
                else
                {
                    forPercentageDiscount += (orderDetail.Price * orderDetail.Quantity);
                }
            }
            var total = forPercentageDiscount + NoPercentageDiscount;
            var percentageDiscount = forPercentageDiscount * percentage;
            var nonPercentageDiscount = Math.Floor(total / 100) * 5;
            var grandTotal = total - (percentageDiscount + nonPercentageDiscount);
            return new InvoiceDisplayed
            {
                OrderPrice=total,
                PercentageDiscount=percentageDiscount,
                NonPercentageDiscount = nonPercentageDiscount,
                PayableTotal = grandTotal,
                order = order
            };
        }
    }
}
