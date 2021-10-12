using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetAllCustomersQuery : IRequest<List<Customer>>
    {
    }

    public class AllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<Customer>>
    {
        private readonly AppDbContext _context;

        public AllCustomersQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _context.Customers.Include(c => c.Orders)
                                        .ThenInclude(o=>o.OrderDetails)
                                        .Include(c => c.UserType)
                                        .ThenInclude(u=>u.Discount)
                                        .ToListAsync();
            return customers;
        }

        //return new List<Customer>();
    }
}
