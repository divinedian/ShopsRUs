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
    public class GetCustomerByNameQuery : IRequest<BaseResponse<Customer>>
    {
        public string UserName { get; set; }
    }

    public class GetCustomerByNameQueryValidator : AbstractValidator<GetCustomerByNameQuery>
    {
        public GetCustomerByNameQueryValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
        }
    }

    public class CustomerByNameQueryHandler : IRequestHandler<GetCustomerByNameQuery, BaseResponse<Customer>>
    {
        private readonly ICustomerDao _customerDao;


        public CustomerByNameQueryHandler(ICustomerDao customerDao)
        {
            _customerDao = customerDao;
        }
        public async Task<BaseResponse<Customer>> Handle(GetCustomerByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _customerDao.GetCustomerByUserName(request.UserName);
                return new BaseResponse<Customer>
                {
                    Data = data,
                    Message = "successful"
                };
            }
            catch(Exception e)
            {
                return new BaseResponse<Customer>
                {
                    Message = e.Message
                };
            }
        }
    }
}
