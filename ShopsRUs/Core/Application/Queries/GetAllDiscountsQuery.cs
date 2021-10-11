using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetAllDiscountsQuery : IRequest<List<Discount>>
    {
    }

    public class AllDiscountQueryHandler : IRequestHandler<GetAllDiscountsQuery, List<Discount>>
    {
        private readonly AppDbContext _context;

        public AllDiscountQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Discount>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Discounts.Include(d => d.UserType)
                                .ToListAsync();
        }
    }
}
