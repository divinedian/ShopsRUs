using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShopsRUs.Core.Core.Application.Command;
using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Data.Models;
using System.Net;
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

        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromQuery] CreateCategoryCommand query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ProducesResponseType(typeof(BaseResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpPost("CreateItem")]
        public async Task<IActionResult> CreateItem([FromQuery] CreateItemCommand query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ProducesResponseType(typeof(BaseResponse<InvoiceDisplayed>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpPost("GetTotalInvoiceAmount")]
        public async Task<IActionResult> GetTotalInvoiceAmount([FromBody] CreateInvoiceFromBillCommand query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
