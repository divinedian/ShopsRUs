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
        private readonly IMediator _mediator;        private readonly GetDiscountPercentageByTypeQuery _percentageQuery;

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
            else return await calculatePercentageDiscount(order);
        }

        public async Task<decimal> calculatePercentageDiscount(Order order)
        {
            decimal forDiscount = 0;
            decimal NoDiscount = 0;
            decimal total = 0;

            var percentage = await _mediator.Send(new GetDiscountPercentageByTypeQuery { UserType = order.Customer.UserType.Title });
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.Item.Category.CategoryName == "Grocery")
                {
                    NoDiscount += (orderDetail.Price * orderDetail.Quantity);
                }
                else
                {
                    forDiscount += (orderDetail.Price * orderDetail.Quantity);
                }
            }
            total = forDiscount + NoDiscount - (forDiscount * percentage);
            var grandTotal = total - (Math.Floor(total / 100) * 5);
            return grandTotal;
        }
    }
}
