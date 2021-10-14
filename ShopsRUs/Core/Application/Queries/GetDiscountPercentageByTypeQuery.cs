using FluentValidation;
using MediatR;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetDiscountPercentageByTypeQuery : IRequest<BaseResponse<decimal>>
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
    public class DiscountPercentageByTypeQueryHandler : IRequestHandler<GetDiscountPercentageByTypeQuery, BaseResponse<decimal>>
    {
        private readonly IDiscountDao _discountDao;

        public DiscountPercentageByTypeQueryHandler(IDiscountDao discountDao)
        {
            _discountDao = discountDao;
        }
        public async Task<BaseResponse<decimal>> Handle(GetDiscountPercentageByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var resp = await _discountDao.GetDiscountPercentage(request.DiscountType);
                return new BaseResponse<decimal>
                {
                    Data = resp,
                    Message = "successful"
                };
            }
            catch(Exception e)
            {
                return new BaseResponse<decimal>
                {
                    Message = e.Message,
                };
            }
        }        
    }
}
