using FluentAssertions;
using NUnit.Framework;
using ShopsRUs.Core.Core.Application.Queries;
using System.Threading.Tasks;

namespace ShopsRUs.Test.QueryTests
{
    using static Testing;
    public class GetTotalInvoiceFromBillQueryTest :TestBase
    {
        [Test]
        public async Task ShouldGetCorrectTotalWithCorrectID()
        {
            WithCustomers();
            var query = new GetTotalInvoiceAmountFromBillQuery
            {
                orderID = "23456"
            };
            var result = await SendAsync(query);

            result.Should().Be(1250);
        }

        [Test]
        public async Task ShouldNotGetCorrectTotalWithInCorrectID()
        {
            WithCustomers();
            var query = new GetTotalInvoiceAmountFromBillQuery
            {
                orderID = "2345"
            };
            var result = await SendAsync(query);

            result.Should().Be(0);
        }
    }
}
