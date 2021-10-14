using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetCustomerByIDQuery : IRequest<BaseResponse<Customer>>
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
    public class CustomerByIdHandler : IRequestHandler<GetCustomerByIDQuery, BaseResponse<Customer>>
    {
        private readonly ICustomerDao _customerDao;

        public CustomerByIdHandler(ICustomerDao customerDao)
        {
            _customerDao = customerDao;
        }
        public async Task<BaseResponse<Customer>> Handle(GetCustomerByIDQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var resp = await _customerDao.GetCustomerById(request.CustomerId);
                return new BaseResponse<Customer>
                {
                    Data = resp,
                    Message = "customer retrieved successfully"
                };
            }
            catch (Exception e)
            {
                return new BaseResponse<Customer>
                {
                    Message = e.Message
                };
            }
        }
    }
}
