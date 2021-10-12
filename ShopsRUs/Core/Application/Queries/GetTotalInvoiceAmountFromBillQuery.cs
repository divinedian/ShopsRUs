using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetTotalInvoiceAmountFromBillQuery : IRequest<decimal>
    {
        public string orderID { get; set; }
    }

    public class TotalInvoiceAmountFromBillHandler : IRequestHandler<GetTotalInvoiceAmountFromBillQuery, decimal>
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;        
        private readonly GetDiscountPercentageByTypeQuery _percentageQuery;

        public TotalInvoiceAmountFromBillHandler(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public class GetTotalInvoiceAmountFromBillQueryValidator : AbstractValidator<GetTotalInvoiceAmountFromBillQuery>
        {
            public GetTotalInvoiceAmountFromBillQueryValidator()
            {
                RuleFor(x => x.orderID).NotNull().NotEmpty();
            }
        }

        public async Task<decimal> Handle(GetTotalInvoiceAmountFromBillQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.Include(o => o.Customer)
                           .ThenInclude(c=>c.UserType)
                           .ThenInclude(u=>u.Discount)
                           .Include(o => o.OrderDetails)
                           .ThenInclude(o=>o.Item)
                           .ThenInclude(i=>i.Category)
                           .Where(o => o.OrderId == request.orderID)
                           .SingleOrDefaultAsync();
            if (order == null) return 0;
            else return await CalculatePercentageDiscount(order);
        }

        public int CalculateYearsSinceRegistration(DateTime dateRegister)
        {
            return dateRegister.Year - DateTime.Now.Year;
        }

        public async Task<decimal> CalculatePercentageDiscount(Order order)
        {
            decimal forPercentageDiscount = 0;
            decimal NoPercentageDiscount = 0;
            decimal percentage = 0;
            var discount = await GetDiscount.GetDiscountFromType(order.Customer.UserType.Title,_context);

            if (CalculateYearsSinceRegistration(order.Customer.DateRegistered) >= discount.Duration)
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
            var grandTotal = total - (percentageDiscount+nonPercentageDiscount);
            return grandTotal;
        }
    }
}
