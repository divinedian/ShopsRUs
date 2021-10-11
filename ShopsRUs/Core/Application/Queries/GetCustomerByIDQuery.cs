using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetCustomerByIDQuery : IRequest<Customer>
    {
        public string CustomerId { get; set; }
    }

    public class GetCustomerByIDQueryValidator : AbstractValidator<GetCustomerByIDQuery>
    {
        public GetCustomerByIDQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
        }
    }
    public class CustomerByIdHandler : IRequestHandler<GetCustomerByIDQuery, Customer>
    {
        private readonly AppDbContext _context;
        
        public CustomerByIdHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Customer> Handle(GetCustomerByIDQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.Include(c => c.Orders)
                               .Include(c => c.UserType)
                               .ThenInclude(u => u.Discount)
                               .Where(c => c.CustomerId == request.CustomerId)
                               .SingleOrDefaultAsync();
        }
    }
}
