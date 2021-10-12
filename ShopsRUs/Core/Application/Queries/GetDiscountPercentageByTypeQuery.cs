using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetDiscountPercentageByTypeQuery : IRequest<(decimal,string)>
    {
        public string DiscountType { get; set; }
    }

    public class GetDiscountPercentageByTypeQueryValidator : AbstractValidator<GetDiscountPercentageByTypeQuery>
    {
        public GetDiscountPercentageByTypeQueryValidator()
        {
            RuleFor(x => x.DiscountType).NotNull().NotEmpty();
        }
    }
    public class DiscountPercentageByTypeQueryHandler : IRequestHandler<GetDiscountPercentageByTypeQuery, (decimal,string)>
    {
        private readonly AppDbContext _context;

        public DiscountPercentageByTypeQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<(decimal,string)> Handle(GetDiscountPercentageByTypeQuery request, CancellationToken cancellationToken)
        {
            var discount =await GetDiscount.GetDiscountFromType(request.DiscountType,_context);
            if (discount != null)
                return (discount.Percentage,"");
            return (0,"discountType does not exist");
        }        
    }
    public static class GetDiscount
    {
        public static async Task<Discount> GetDiscountFromType(string discountType, AppDbContext _context)
        {
            return await _context.Discounts.Include(d => d.UserType)
                                        .Where(d => d.DiscountType == discountType)
                                        .SingleOrDefaultAsync();
        }
    }
}
