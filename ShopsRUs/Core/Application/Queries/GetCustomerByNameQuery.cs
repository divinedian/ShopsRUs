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
    public class GetCustomerByNameQuery :IRequest<Customer>
    {
        public string Name { get; set; }
    }

    public class GetCustomerByNameQueryValidator : AbstractValidator<GetCustomerByNameQuery>
    {
        public GetCustomerByNameQueryValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }

    public class CustomerByNameQueryHandler : IRequestHandler<GetCustomerByNameQuery, Customer>
    {
        private readonly AppDbContext _context;
        public CustomerByNameQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async  Task<Customer> Handle(GetCustomerByNameQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.Include(c => c.Orders)
                               .Include(c => c.UserType)
                               .ThenInclude(u => u.Discount)
                               .Where(c => c.UserName == request.Name)
                               .SingleOrDefaultAsync();
        }
    }
}
