using MediatR;
using ShopsRUs.Core.Core.Dao;
using ShopsRUs.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Queries
{
    public class GetAllDiscountsQuery : IRequest<BaseResponse<List<Discount>>>
    {
    }

    public class AllDiscountQueryHandler : IRequestHandler<GetAllDiscountsQuery, BaseResponse<List<Discount>>>
    {
        private readonly IDiscountDao _discountDao;

        public AllDiscountQueryHandler(IDiscountDao discountDao)
        {
            _discountDao = discountDao;
        }
        public async Task<BaseResponse<List<Discount>>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
        {
            var resp =  await _discountDao.GetAllDiscount();
            return new BaseResponse<List<Discount>>
            {
                Data = resp
            };
        }
    }
}
