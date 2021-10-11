using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopsRUs.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetDiscountPercentageByTypeQuery : IRequest<decimal>
    {
        public string UserType { get; set; }
    }

    public class GetDiscountPercentageByTypeQueryValidator : AbstractValidator<GetDiscountPercentageByTypeQuery>
    {
        public GetDiscountPercentageByTypeQueryValidator()
        {
            RuleFor(x => x.UserType).NotNull().NotEmpty();
        }
    }
    public class DiscountPercentageByTypeQueryHandler : IRequestHandler<GetDiscountPercentageByTypeQuery, decimal>
    {
        private readonly AppDbContext _context;

        public DiscountPercentageByTypeQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> Handle(GetDiscountPercentageByTypeQuery request, CancellationToken cancellationToken)
        {
            var discount = await _context.Discounts.Include(d => d.UserType)
                                        .Where(d => d.UserType.Title == request.UserType)
                                        .SingleOrDefaultAsync();
            if (discount != null)
                return discount.Percentage;
            return 0;
        }
    }
}
