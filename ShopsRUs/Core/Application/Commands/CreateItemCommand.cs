using AutoMapper;
using FluentValidation;
using MediatR;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Commands
{
    public class CreateItemCommand : IRequest<BaseResponse<bool>>
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, BaseResponse<bool>>
    {
        private readonly IInvoiceDao _invoiceDao;
        private readonly IMapper _mapper;

        public CreateItemCommandHandler(IInvoiceDao invoiceDao, IMapper mapper)
        {
            _invoiceDao = invoiceDao;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var items = _mapper.Map<Item>(request);
                await _invoiceDao.CreateItem(items);
                return new BaseResponse<bool>
                {
                    Data = true,
                    Message = "Item added"
                };
            }      
            catch(Exception e)
            {
                return new BaseResponse<bool>
                {
                    Message = e.Message
                };
            }
        }
    }
}
