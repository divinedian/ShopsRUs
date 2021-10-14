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
    public class AddDiscountTypeCommand :IRequest<BaseResponse<bool>>
    {
        public string DiscountType { get; set; }
        public decimal Percentage { get; set; }
        public int Duration { get; set; }
    }

    public class AddDiscountTypeCommandValidator : AbstractValidator<AddDiscountTypeCommand>
    {
        public AddDiscountTypeCommandValidator()
        {
            RuleFor(x => x.DiscountType).NotNull().NotEmpty();
            RuleFor(x => x.Percentage).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1).WithMessage("Input the decimal value of percentage");
            RuleFor(x => x.Percentage).NotEmpty();
        }
    }  

    public class DiscountTypeCommandHandler : IRequestHandler<AddDiscountTypeCommand, BaseResponse<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IDiscountDao _discountDao;

        public DiscountTypeCommandHandler(IMapper mapper, IDiscountDao discountDao)
        {
            _mapper = mapper;
            _discountDao = discountDao;
        }
        public async Task<BaseResponse<bool>> Handle(AddDiscountTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var discountTypeToAdd = _mapper.Map<Discount>(request);
                await _discountDao.AddDiscount(discountTypeToAdd);
                return new BaseResponse<bool>
                {
                    Message = "new Discount type added successfully",
                    Data = true
                };
            }
            catch(Exception e)
            {
                return new BaseResponse<bool>
                {
                    Message = e.Message,
                    Data = false
                };
            }
        }
    }
}
