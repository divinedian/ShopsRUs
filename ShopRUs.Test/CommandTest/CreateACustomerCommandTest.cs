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
    public class CreateACustomerCommandTest: TestBase
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
            var (isCreated, message) = await SendAsync(query);

            isCreated.Should().BeTrue();
            message.Should().Be("User Created");

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

            var (isCreated, message) = await SendAsync(query);
            isCreated.Should().BeTrue();
            message.Should().Be("User Created");
            var (isReCreated, message2) = await SendAsync(query);
            isReCreated.Should().BeFalse();
            message2.Should().Be("Username already exists");
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

            var (isCreated, message) = await SendAsync(query);
            isCreated.Should().BeFalse();
            message.Should().Be("There is no usertype with title: Affiliate");
        }
        }
}
