using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Core.Core.Application.Queries;
using ShopsRUs.Data.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly GetAllDiscountsQuery _allDiscountsQuery;
        public DiscountController(IMediator mediator, GetAllDiscountsQuery allDiscountsQuery)
        {
            _allDiscountsQuery = allDiscountsQuery;
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(BaseResponse<List<Discount>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpGet("GetAllDiscounts")]
        public async Task<IActionResult> GetAllDiscounts()
        {
            return Ok(await _mediator.Send(_allDiscountsQuery));
        }

        [ProducesResponseType(typeof(BaseResponse<decimal>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpGet("GetDiscountPercentage")]
        public async Task<IActionResult> GetDiscountPercentageByType([FromQuery]GetDiscountPercentageByTypeQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ProducesResponseType(typeof(BaseResponse<bool>),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse),(int)HttpStatusCode.BadRequest)]
        [HttpPost("AddDiscountType")]
        public async Task<IActionResult> AddDiscountType([FromQuery] AddDiscountTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
