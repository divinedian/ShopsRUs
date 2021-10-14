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
                DiscountType = "Customer"
            };

            var response = await SendAsync(query);

            response.Data.Should().Be(0.25M);
        }

        [Test]
        public async Task ShouldNotGetDisCountPercentWithWrongType()
        {
            WithCustomers();

            var query = new GetDiscountPercentageByTypeQuery
            {
                DiscountType = "Customerr"
            };

            var resp = await SendAsync(query);

            resp.Data.Should().Be(0);
        }
    }
}
