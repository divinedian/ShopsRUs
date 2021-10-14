using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using Respawn;
using ShopsRUs.Core;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopsRUs.Test
{
    [SetUpFixture]
    public class Testing
    {
        public static IConfiguration _configuration;
        public static IServiceCollection _services;
        public static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkPoint;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            _services = new ServiceCollection();
            var startup = new Startup(_configuration);
            _services.AddSingleton(s => new Mock<IHostEnvironment>().Object);
            _services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.ApplicationName == "ShopsRUs.Core"
            && w.EnvironmentName == "Development"));
            _services.AddScoped(_ => _configuration);

            _services.AddLogging();

            startup.ConfigureServices(_services);

            _scopeFactory = _services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            _checkPoint = new Checkpoint
            {
                TablesToIgnore = new[]
                {
                    "_EFMigrationsHistory"
                }
            };
        }

        public static async Task ResetDbState()
        {
            await _checkPoint.Reset(_configuration.GetConnectionString("TestingConnection"));
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        public async static Task Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            context.Remove(entity);
            await context.SaveChangesAsync();
        }


        public static async Task AddRangeAsync<TEntity>(List<TEntity> entities)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            await context.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public static void WithCustomers()
        {
            AddRangeAsync(SeedCategories()).GetAwaiter().GetResult();
            AddRangeAsync(SeedCustomers()).GetAwaiter().GetResult();
        }

        public static void WithUserType()
        {
            AddRangeAsync(SeedUserType()).GetAwaiter().GetResult();
        }

        public static void WithCategories()
        {
            AddRangeAsync(SeedCategories()).GetAwaiter().GetResult();
        }

        public static void WithOrder()
        {
            AddRangeAsync(SeedOrders()).GetAwaiter().GetResult();
        }

        private static List<Customer> SeedCustomers()
        {
            var userTypes = SeedUserType();
            return new List<Customer>
            {
                new Customer
                {                    
                    DateRegistered = DateTime.Now,
                    CustomerId = "12345",
                    FirstName = "Diana",
                    LastName = "Ekwere",
                    UserName = "Divinedee",
                    UserTypeId = userTypes[0].UserTypeId,
                    UserType = userTypes[0],
                    Orders = SeedOrders(),
                }
            };
        }

        private static List<UserType> SeedUserType()
        {
            var discounts = new List<Discount>
            {
                new Discount
                {
                    DiscountId = "54321",
                    DiscountType = "Customer",
                    Percentage = 0.25M,
                    Duration = 0
                },
                new Discount
                {
                    DiscountId = "54322",
                    DiscountType = "Employee",
                    Percentage = 0.05M,
                    Duration = 1
                },
                new Discount
                {
                    DiscountId = "54323",
                    DiscountType = "Affiliate",
                    Percentage = 0.1M,
                    Duration = 0
                }
            };
            return new List<UserType>
            {
                new UserType
                {
                    Title = "Customer",
                    UserTypeId = "2435",
                    Discount = discounts[0],
                    DiscountId = discounts[0].DiscountId
                },
                new UserType
                {
                    Title = "Regular",
                    UserTypeId = "2436",
                    Discount = discounts[1],
                    DiscountId = discounts[1].DiscountId
                },
                new UserType
                {
                    Title = "Affiliate",
                    UserTypeId = "2437",
                    Discount = discounts[2],
                    DiscountId = discounts[2].DiscountId
                },
            };
        }

        private static List<Category> SeedCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    CategoryId =  "616256a384b3f9a1de71ef8f",
                    CategoryName = "footwears",
                    Description = "Amet mollit sit culpa et irure proident. Eu ex proident commodo cupidatat minim voluptate ",
                    Items = new List<Item>
                    {
                        new Item
                        {
                            CategoryId = "616256a384b3f9a1de71ef8f",
                            Description = "asdfghgjhhgfsdaAFSDGFNHGFGDFHMFGDFSD",
                            InStock = true,
                            IsItemOfTheWeek = true,
                            ItemId = "2390",
                            Price = 56,
                            Name = "Sneakers",
                            ImageUrl = "sdghjkhgdfsbnbf"
                        },
                        new Item
                        {
                            CategoryId = "616256a384b3f9a1de71ef8f",
                            Description = "asdfghgjhhgfsdaAFSDGFNHGFGDFHMFGDFSD",
                            InStock = true,
                            IsItemOfTheWeek = false,
                            ItemId = "2391",
                            Price = 234,
                            Name = "Trainers",
                            ImageUrl = "sdghjkhgdfsbnbf"
                        }
                    }
                },
                new Category
                {
                    CategoryId =  "616256a384b3f9a1de71effh",
                    CategoryName = "Grocery",
                    Description = "dontte mollit sit culpa et irure proident. Eu ex proident commodo cupidatat minim voluptate ",
                    Items = new List<Item>
                    {
                        new Item
                        {
                            Description = "asdfghgjhhgfsdaAFSDGFNHGFGDFHMFGDFSD",
                            InStock = true,
                            IsItemOfTheWeek = true,
                            ItemId = "2389",
                            Price = 450,
                            Name = "Fish",
                            ImageUrl = "sdghjkhgdfsbnbf"
                        },
                        new Item
                        {
                            Description = "asdfghgjhhgfsdaAFSDGFNHGFGDFHMFGDFSD",
                            InStock = true,
                            IsItemOfTheWeek = false,
                            ItemId = "2383",
                            Price = 500,
                            Name = "Meat",
                            ImageUrl = "sdghjkhgdfsbnbf"
                        }
                    }
                }
            };
        }

        private static List<Order> SeedOrders()
        {
            var categories = SeedCategories();
            return new List<Order>
            {
                new Order
                {
                    OrderId = "23456",
                    OrderPlaced = DateTime.Now,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            ItemId = categories[0].Items[0].ItemId,                            
                            OrderDetailId = "0987",
                            Quantity = 10,
                            Price = categories[0].Items[0].Price
                        },
                        new OrderDetail
                        {
                            ItemId = categories[1].Items[0].ItemId,
                            OrderDetailId = "0988",
                            Quantity = 2,
                            Price = categories[1].Items[0].Price
                        }
                    }
                },
                new Order
                {
                    OrderId = "23457",
                    OrderPlaced = DateTime.Now,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail
                        {                            
                            ItemId = categories[1].Items[1].ItemId,
                            OrderDetailId = "0989",
                            Quantity = 3,
                            Price = categories[1].Items[1].Price
                        }
                    }
                },
                new Order
                {
                    OrderId = "23458",
                    OrderPlaced = DateTime.Now,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            ItemId = categories[0].Items[1].ItemId,
                            OrderDetailId = "0991",
                            Quantity = 1,
                            Price = categories[0].Items[1].Price
                        },
                        new OrderDetail
                        {
                            ItemId = categories[1].Items[0].ItemId,
                            OrderDetailId = "0992",
                            Quantity = 5,
                            Price = categories[1].Items[0].Price
                        }
                    }
                }
            };
        }

        public static void RemoveProducts()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var custs = context.Customers;
            foreach (var cust in custs)
            {
                foreach(var ord in cust.Orders)
                {
                    foreach(var ordD in ord.OrderDetails)
                    {
                        context.OrderDetails.Remove(ordD);
                    }
                    context.Orders.Remove(ord);
                }
                context.Customers.Remove(cust);
            }

            var cat = context.Categories;
            foreach(var c in cat)
            {
                foreach(var i in c.Items)
                {
                    context.Items.Remove(i);
                }
                context.Categories.Remove(c);
            }
            
        }
    }
}
