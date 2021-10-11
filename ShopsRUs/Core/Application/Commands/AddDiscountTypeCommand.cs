using AutoMapper;
using FluentValidation;
using MediatR;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Commands
{
    public class AddDiscountTypeCommand :IRequest<bool>
    {
        public string DiscountType { get; set; }
        public decimal Percentage { get; set; }
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

    public class DiscountTypeCommandHandler : IRequestHandler<AddDiscountTypeCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public DiscountTypeCommandHandler(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<bool> Handle(AddDiscountTypeCommand request, CancellationToken cancellationToken)
        {
            var discountTypeToAdd = _mapper.Map<Discount>(request);
            discountTypeToAdd.UserType = new UserType();
            discountTypeToAdd.UserType.Title = discountTypeToAdd.DiscountType;
            discountTypeToAdd.UserType.UserTypeId = Guid.NewGuid().ToString();

            if (_context.Discounts.Any(d => d.DiscountType == request.DiscountType))
            {
                return false;
            }

            _context.Discounts.Add(discountTypeToAdd);
            return await SaveAsync();
        }

        private async Task<bool> SaveAsync()
        {
            var ValueReturned = false;
            if (await _context.SaveChangesAsync() > 0)
                ValueReturned = true;
            else
                ValueReturned = false;
            return ValueReturned;
        }
    }
}
