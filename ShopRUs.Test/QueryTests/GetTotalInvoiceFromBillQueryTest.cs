using FluentAssertions;
using NUnit.Framework;
using ShopsRUs.Core.Core.Application.Commands;
using System.Collections.Generic;
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
            //WithCategories();
            var query = new CreateInvoiceFromBillCommand
            {
                CustomerId = "12345",
                orderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Item = "Sneakers",
                        Quantities = 2
                    },
                    new OrderItem
                    {
                        Item = "Fish",
                        Quantities = 1
                    },
                    new OrderItem
                    {
                        Item = "Trainers",
                        Quantities = 1
                    },
                }
            };
            var result = await SendAsync(query);
            result.Data.OrderPrice.Should().Be(796);
            result.Data.PercentageDiscount.Should().Be(86.5M);
            result.Data.NonPercentageDiscount.Should().Be(35);
            result.Data.PayableTotal.Should().Be(674.5M);
        }        
    }
}
