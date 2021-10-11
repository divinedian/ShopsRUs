using FluentAssertions;
using NUnit.Framework;
using ShopsRUs.Core.Core.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Test
{
    using static Testing;
    public class GetCustomerByIdQueryTest :TestBase
    {
        [Test]
        public async Task ShouldReturnCustomerWithIdSupplied()
        {
            WithCustomers();
            var Query = new GetCustomerByIDQuery
            {
                CustomerId = "12345"
            };
            var result = await SendAsync(Query);

            result.Should().NotBeNull();
            result.CustomerId.Should().Be("12345");
            result.FirstName.Should().Be("Diana");
            result.LastName.Should().Be("Ekwere");
            result.UserName.Should().Be("Divinedee");
            result.Orders.Count.Should().Be(3);
        }

        [Test]
        public async Task ShouldNotReturnCustomerWithWrongIdSupplied()
        {
            WithCustomers();
            var Query = new GetCustomerByIDQuery
            {
                CustomerId = "1234"
            };
            var result = await SendAsync(Query);

            result.Should().BeNull();
        }
    }
}
