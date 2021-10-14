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
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly GetAllCustomersQuery _query;

        public CustomerController(IMediator mediator, GetAllCustomersQuery customersQuery)
        {
            _mediator = mediator;
            _query = customersQuery;
        }

        [ProducesResponseType(typeof(BaseResponse<List<Customer>>),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            return Ok(await _mediator.Send(_query));
        }

        [ProducesResponseType(typeof(BaseResponse<Customer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpGet("GetCustomerByID")]
        public async Task<IActionResult> GetCustomerById([FromQuery] GetCustomerByIDQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ProducesResponseType(typeof(BaseResponse<Customer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpGet("GetCustomerByUserName")]
        public async Task<IActionResult> GetCustomerByUserName([FromQuery] GetCustomerByNameQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateACustomer([FromQuery] CreateACustomerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
