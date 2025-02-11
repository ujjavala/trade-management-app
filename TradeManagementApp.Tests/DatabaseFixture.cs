using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Persistence;

public class DatabaseFixture : IDisposable
{
    public DataContext Context { get; private set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test run
            .Options;
        Context = new DataContext(options);

        // Seed data
        Context.Accounts.AddRange(new List<Account>
        {
            new Account { Id = 1, FirstName = "John", LastName = "Doe" },
            new Account { Id = 2, FirstName = "Jane", LastName = "Doe" }
        });

        Context.Trades.AddRange(new List<Trade>
        {
            new Trade { Id = 1, Status = TradeStatus.Placed, BuyOrSell = "Buy", SecurityCode = "AAPL" },
            new Trade { Id = 2, Status = TradeStatus.Executed, BuyOrSell = "Sell", SecurityCode = "GOOGL" }
        });

        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}