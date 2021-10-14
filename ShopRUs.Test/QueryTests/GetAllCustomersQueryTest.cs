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
    public class GetAllCustomersQueryTest :TestBase
    {
        [Test]
        public async Task ShouldGetAllCustomersFromDB()
        {
            WithCustomers();

            var Query = new GetAllCustomersQuery();
            var result = await SendAsync(Query);

            result.Data.Count.Should().NotBe(0);
            result.Data.Count.Should().Be(1);
        }

        [Test]
        public async Task ShouldNotGetAnythingWithEmptyDb()
        {
            var Query = new GetAllCustomersQuery();
            var result = await SendAsync(Query);

            result.Data.Count.Should().Be(0);
        }
    }
}
