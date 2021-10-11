using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopsRUs.Data.Models;

namespace ShopsRUs.Data
{
    public class Seeding
    {
        private static string basePath = Environment.CurrentDirectory;
        private static string relativePath = "../ShopsRUs.Data/SeedData/";
        private static string path = Path.GetFullPath(relativePath, basePath);
        
        public static async Task EnsureCreated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Customers.Any())
            {
                //Seed Category
                var categoryList = FetchAndDeserializeSeedData<Category>(File.ReadAllText(path + "Category.json"));
                await context.Categories.AddRangeAsync(categoryList);

                //Seed items
                var itemsList = FetchAndDeserializeSeedData<Item>(File.ReadAllText(path + "Item.json"));
                await context.Items.AddRangeAsync(itemsList);

                //Seed Discounts
                var discountList = FetchAndDeserializeSeedData<Discount>(File.ReadAllText(path + "Discount.json"));
                await context.Discounts.AddRangeAsync(discountList);

                //Seed UserType
                var userTypeList = FetchAndDeserializeSeedData<UserType>(File.ReadAllText(path + "UserType.json"));
                await context.UserTypes.AddRangeAsync(userTypeList);

                //Seed Customers
                var customerList = FetchAndDeserializeSeedData<Customer>(File.ReadAllText(path + "Customer.json"));
                await context.Customers.AddRangeAsync(customerList);

                //Seed Orders
                var ordersList = FetchAndDeserializeSeedData<Order>(File.ReadAllText(path + "Order.json"));
                await context.Orders.AddRangeAsync(ordersList);

                //Seed OrderDetails
                var orderDetailsList = FetchAndDeserializeSeedData<OrderDetail>(File.ReadAllText(path + "OrderDetail.json"));
                foreach(var detail in orderDetailsList)
                {
                    foreach(var item in itemsList)
                    {
                        if (detail.ItemId == item.ItemId)
                            detail.Price = item.Price;
                    }
                }
                await context.OrderDetails.AddRangeAsync(orderDetailsList);

                await context.SaveChangesAsync();
            }
        }

        private static List<T> FetchAndDeserializeSeedData<T>(string fetchfrom)
        {
            var deserialized = JsonSerializer.Deserialize<List<T>>(fetchfrom, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return deserialized;
        }
    }
}
