using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Core.Core.Application.Queries;
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

        [HttpGet("GetAllDiscounts")]
        public async Task<IActionResult> GetAllDiscounts()
        {
            var discountsretrieved = await _mediator.Send(_allDiscountsQuery);
            if (discountsretrieved.Count == 0) return BadRequest("You have no discount on your db");
            return Ok(discountsretrieved);
        }

        [HttpGet("GetDiscountPercentage")]
        public async Task<IActionResult> GetDiscountPercentageByType([FromQuery]GetDiscountPercentageByTypeQuery query)
        {
            var discountPercentage = await _mediator.Send(query);
            return Ok(discountPercentage);
        }

        [HttpPost("AddDiscountType")]
        public async Task<IActionResult> AddDiscountType([FromQuery] AddDiscountTypeCommand command)
        {
            var resp = await _mediator.Send(command);
            if (!resp) return BadRequest("DiscountType not added");
            return Ok();
        }
    }
}
