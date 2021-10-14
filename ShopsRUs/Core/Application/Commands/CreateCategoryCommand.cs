using AutoMapper;
using FluentValidation;
using MediatR;
using ShopsRUs.Core.Core.Application.Command;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Command
{
    public class CreateCategoryCommand:IRequest<BaseResponse<string>>
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}

public class CreateCategoryCommandValidator: AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryName).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();
        //RuleFor(x => x.Items).NotEmpty().NotNull();
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, BaseResponse<string>>
    {
        private readonly IInvoiceDao _invoiceDao;
        private readonly IMapper _mapper;
        public CreateCategoryCommandHandler(IMapper mapper, IInvoiceDao invoiceDao)
        {
            _invoiceDao = invoiceDao;
            _mapper = mapper;
        }
        public async Task<BaseResponse<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = _mapper.Map<Category>(request);
                await _invoiceDao.CreateCategory(category);
                return new BaseResponse<string>
                {
                    Data = category.CategoryId,
                    Message = $"Category successfully created with id: {category.CategoryId}"
                };
            } 
            catch(Exception e)
            {
                return new BaseResponse<string>
                {
                    Message = e.Message
                };
            }
        }
    }
}
