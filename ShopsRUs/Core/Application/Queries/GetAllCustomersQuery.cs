using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetAllCustomersQuery : IRequest<BaseResponse<List<Customer>>>
    {
    }

    public class AllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, BaseResponse<List<Customer>>>
    {
        private readonly AppDbContext _context;
        private readonly ICustomerDao _customerDao;

        public AllCustomersQueryHandler(AppDbContext context, ICustomerDao customerDao)
        {
            _context = context;
            _customerDao = customerDao;
        }
        public Task<BaseResponse<List<Customer>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = _customerDao.GetAllCustomers().Result;
            return Task.FromResult(new BaseResponse<List<Customer>>
            {
                Message = "Customers retrieved successfully",
                Data = customers
            });
        }
    }
}
