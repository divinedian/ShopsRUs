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
    public class GetCustomerByNameQueryTest : TestBase
    {
        [Test]
        public async Task ShouldGetCustomerFromDb()
        {
            WithCustomers();

            var query = new GetCustomerByNameQuery
            {
                UserName = "Divinedee"
            };
            var result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Data.UserName.Should().Be("Divinedee");
        }

        [Test]
        public async Task ShouldNotGetCustomerWithWrongUserName()
        {
            var query = new GetCustomerByNameQuery
            {
                UserName = "Divinedian"
            };
            var result = await SendAsync(query);

            result.Data.Should().BeNull();
        }
    }
}
