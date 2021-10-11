using FluentAssertions;
using NUnit.Framework;
using ShopsRUs.Core.Core.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Test.QueryTests
{
    using static Testing;
    public class GetDiscountPercentageByTypeQueryTest : TestBase
    {
        [Test]
        public async Task ShouldGetDisCountPercentWithCorrectType()
        {
            WithCustomers();

            var query = new GetDiscountPercentageByTypeQuery
            {
                UserType = "Customer"
            };

            var result = await SendAsync(query);

            result.Should().Be(0.25M);
        }

        [Test]
        public async Task ShouldNotGetDisCountPercentWithWrongType()
        {
            WithCustomers();

            var query = new GetDiscountPercentageByTypeQuery
            {
                UserType = "Customerr"
            };

            var result = await SendAsync(query);

            result.Should().Be(0);
        }
    }
}
