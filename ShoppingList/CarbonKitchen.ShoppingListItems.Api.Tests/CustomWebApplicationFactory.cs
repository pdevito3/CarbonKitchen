using CarbonKitchen.ShoppingListItems.Api.Controllers;
using CarbonKitchen.ShoppingListItems.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace CarbonKitchen.ShoppingListItems.Api.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
            .ConfigureAppConfiguration((context, b) => {
                context.HostingEnvironment.ApplicationName = typeof(ShoppingListItemsController).Assembly.GetName().Name;
            })
            .ConfigureServices(services =>
            {
                // Remove the app's ShoppingListItemDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ShoppingListItemDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ShoppingListItemDbContext using an in-memory database for testing.
                services.AddDbContext<ShoppingListItemDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDb");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ShoppingListItemDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        db.RemoveRange(db.ShoppingListItems);
                        // Seed the database with test data.
                        //Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }
    }
}
