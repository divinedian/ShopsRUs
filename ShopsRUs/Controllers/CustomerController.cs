using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopsRUs.Core.Core.Application.Commands;
using ShopsRUs.Core.Core.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            //var query = new GetAllCustomersQuery();
            var Customers = await _mediator.Send(_query);
            if (Customers.Count == 0) return BadRequest("You have no customers right now");
            return Ok(Customers);
        }

        [HttpGet("GetCustomerByID")]
        public async Task<IActionResult> GetCustomerById([FromQuery] GetCustomerByIDQuery query)
        {
            var Customer = await _mediator.Send(query);
            if (Customer == null) return BadRequest($"You have no customer with Id: {query.CustomerId}");
            return Ok(Customer);
        }

        [HttpGet("GetCustomerByName")]
        public async Task<IActionResult> GetCustomerByName([FromQuery] GetCustomerByNameQuery query)
        {
            var Customer = await _mediator.Send(query);
            if (Customer == null) return BadRequest($"You have no customer with name: {query.Name}");
            return Ok(Customer);
        }

        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateACustomer([FromQuery] CreateACustomerCommand command)
        {
            var (isCreated,message)= await _mediator.Send(command);
            if (!isCreated) return BadRequest($"{message}");
            return Ok();
        }
    }
}
