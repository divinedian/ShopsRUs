using AutoMapper;
using FluentValidation;
using MediatR;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Commands
{
    public class CreateInvoiceFromBillCommand : IRequest<BaseResponse<InvoiceDisplayed>>
    {
        public string CustomerId { get; set; }
        public List<OrderItem> orderItems { get; set; }

    }

    public class GetTotalInvoiceAmountFromBillQueryValidator : AbstractValidator<CreateInvoiceFromBillCommand>
    {
        public GetTotalInvoiceAmountFromBillQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
            RuleFor(x => x.orderItems).NotEmpty();
        }
    }    

    public class TotalInvoiceAmountFromBillHandler : IRequestHandler<CreateInvoiceFromBillCommand, BaseResponse<InvoiceDisplayed>>
    {
        private readonly IInvoiceDao _invoiceDao;
        private readonly IMapper _mapper;
        private readonly ICustomerDao _customerDao;

        public TotalInvoiceAmountFromBillHandler(IMapper mapper, IInvoiceDao invoiceDao, ICustomerDao customerDao)
        {
            _mapper = mapper;
            _invoiceDao = invoiceDao;
            _customerDao = customerDao;
        }

        public async Task<BaseResponse<InvoiceDisplayed>> Handle(CreateInvoiceFromBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _mapper.Map<Order>(request);
                var (forPercentdiscount, NoPercentdiscount, orderComplete) = await _invoiceDao.AddOrderDetailsToOrder(order, request.orderItems);
                var percentage = await _customerDao.GetCustomersDiscount(request.CustomerId);
                var resp = _invoiceDao.PrepareBill(forPercentdiscount, NoPercentdiscount, percentage, orderComplete);
                return new BaseResponse<InvoiceDisplayed>
                {
                    Data = resp,
                    Message = "Successful"
                };
            }
            catch(Exception e)
            {
                return new BaseResponse<InvoiceDisplayed>
                {
                    Message = e.Message
                };
            }            
        }        
    }

    public class InvoiceDisplayed
    {
        public decimal OrderPrice { get; set; }
        public decimal PercentageDiscount { get; set; }
        public decimal NonPercentageDiscount { get; set; }
        public decimal PayableTotal { get; set; }
        public Order order { get; set; }        
    }

    public class OrderItem
    {
        public string Item { get; set; }
        public int Quantities { get; set; }
    }
}
