using FluentAssertions;
using NUnit.Framework;
using ShopsRUs.Core.Core.Application.Queries;
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

            result.Data.Count.Should().NotBe(0);
        }

        [Test]
        public async Task ShouldNotGetAllDiscountsFromEmptyDb()
        {

            var query = new GetAllDiscountsQuery();

            var result = await SendAsync(query);

            result.Data.Count.Should().Be(0);
        }
    }
}
