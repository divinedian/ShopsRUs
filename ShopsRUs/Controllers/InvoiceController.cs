using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShopsRUs.Core.Core.Application.Queries;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetTotalInvoiceAmount")]
        public async Task<IActionResult> GetTotalInvoiceAmount([FromQuery] GetTotalInvoiceAmountFromBillQuery query)
        {
            var amount = await _mediator.Send(query);
            return Ok(amount);
        }
    }
}
