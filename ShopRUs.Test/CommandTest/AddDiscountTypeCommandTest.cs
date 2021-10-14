using FluentAssertions;
using NUnit.Framework;
using ShopsRUs.Core.Core.Application.Commands;
using System.Threading.Tasks;

namespace ShopsRUs.Test.CommandTest
{
    using static Testing;
    public class AddDiscountTypeCommandTest : TestBase
    {
        [Test]
        public async Task ShouldAddDiscountType()
        {
            var query = new AddDiscountTypeCommand
            {
                DiscountType = "Affiliate",
                Percentage = 0.25M
            };

            var result = await SendAsync(query);
            result.Data.Should().BeTrue();
        }
    }
}
