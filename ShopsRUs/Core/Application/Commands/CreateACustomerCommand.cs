using AutoMapper;
using FluentValidation;
using MediatR;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Commands
{
    public class CreateACustomerCommand : IRequest<BaseResponse<string>>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserTypeTitle { get; set; }
    }

    public class CreateACustomerCommandValidator : AbstractValidator<CreateACustomerCommand>
    {
        public CreateACustomerCommandValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.UserTypeTitle).NotNull().NotEmpty();
        }
    }

    public class ACustomerCommandHandler : IRequestHandler<CreateACustomerCommand, BaseResponse<string>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICustomerDao _customerDao;

        public ACustomerCommandHandler(AppDbContext context, IMapper mapper, ICustomerDao customerDao)
        {
            _customerDao = customerDao;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<string>> Handle(CreateACustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customerToCreate = _mapper.Map<Customer>(request);
                var data = await _customerDao.CreateCustomer(customerToCreate, request.UserTypeTitle);
                return new BaseResponse<string>
                {
                    Data = data,
                    Message = $"User created successfully. See Id as data"
                };
            }
            catch (Exception e)
            {
                return new BaseResponse<string>
                {                    
                    Message = e.Message
                };
            }
        }
    }
}
