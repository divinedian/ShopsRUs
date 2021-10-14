using FluentAssertions;
using NUnit.Framework;
using ShopsRUs.Core.Core.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Test.CommandTest
{
    using static Testing;
    public class CreateACustomerCommandTest : TestBase
    {
        [Test]
        public async Task ShouldCreateACustomer()
        {
            WithUserType();
            var query = new CreateACustomerCommand
            {
                FirstName = "Diana",
                LastName = "Ekwere",
                UserName = "DivineDee",
                UserTypeTitle = "Affiliate"
            };
            var resp = await SendAsync(query);
            resp.Message.Should().Be("User created successfully. See Id as data");
        }

        [Test]
        public async Task ShouldNotAllowDuplicate()
        {
            WithUserType();
            var query = new CreateACustomerCommand
            {
                FirstName = "Diana",
                LastName = "Ekwere",
                UserName = "DivineDee",
                UserTypeTitle = "Affiliate"
            };

            var resp = await SendAsync(query);
            resp.Message.Should().Be("User created successfully. See Id as data");
            //Task.Delay(10000);
            var respAgain = await SendAsync(query);
            respAgain.Message.Should().Be("Username already exists");
        }

        [Test]
        public async Task ShouldNotAllowUserTypeThatDoesNotExists()
        {
            var query = new CreateACustomerCommand
            {
                FirstName = "Diana",
                LastName = "Ekwere",
                UserName = "DivineDee",
                UserTypeTitle = "Affiliate"
            };

             var resp = await SendAsync(query);
            resp.Message.Should().Be($"There is no usertype with title: {query.UserTypeTitle}");
        }
    }
}
