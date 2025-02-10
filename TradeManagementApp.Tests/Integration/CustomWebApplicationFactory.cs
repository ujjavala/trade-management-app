// <copyright file="CustomWebApplicationFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TradeManagementApp.Models;
using TradeManagementApp.Persistence;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DataContext>();

                db.Database.EnsureCreated();

                // Reset the database before each test
                db.Accounts.RemoveRange(db.Accounts);
                db.Trades.RemoveRange(db.Trades);
                db.SaveChanges();

                // Seed the database with test data
                SeedData(db);
            }
        });

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            config.AddJsonFile("appsettings.json")
                  .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                  .AddEnvironmentVariables();
        });
    }

    private void SeedData(DataContext context)
    {
        // Add your seed data here
        context.Accounts.Add(new Account { Id = 1, FirstName = "John", LastName = "Doe" });
        context.Accounts.Add(new Account { Id = 2, FirstName = "Jane", LastName = "Smith" });

        context.Trades.Add(new Trade { Id = 1, AccountId = 1, SecurityCode = "APL", Timestamp = DateTime.UtcNow, Amount = 100, BuyOrSell = "Buy", Status = TradeStatus.Executed });
        context.Trades.Add(new Trade { Id = 2, AccountId = 2, SecurityCode = "GOL", Timestamp = DateTime.UtcNow, Amount = 200, BuyOrSell = "Sell", Status = TradeStatus.Executed });

        for (int i = 3; i <= 52; i++)
        {
            context.Trades.Add(new Trade
            {
                Id = i,
                AccountId = (i % 2) + 1,
                SecurityCode = "SEC" + (i % 100).ToString("D2"),
                Timestamp = DateTime.UtcNow,
                Amount = i * 10m,
                BuyOrSell = i % 2 == 0 ? "Buy" : "Sell",
                Status = TradeStatus.Placed
            });
        }

        context.SaveChanges();
    }
}
