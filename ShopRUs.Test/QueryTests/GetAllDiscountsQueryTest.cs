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
    public class GetAllDiscountsQueryTest : TestBase
    {
        [Test]
        public async Task ShouldGetAllDiscountsFromDb()
        {
            WithCustomers();

            var query = new GetAllDiscountsQuery();

            var result = await SendAsync(query);

            result.Count.Should().NotBe(0);
        }

        [Test]
        public async Task ShouldNotGetAllDiscountsFromEmptyDb()
        {

            var query = new GetAllDiscountsQuery();

            var result = await SendAsync(query);

            result.Count.Should().Be(0);
        }
    }
}
