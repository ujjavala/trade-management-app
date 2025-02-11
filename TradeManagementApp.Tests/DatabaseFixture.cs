using Microsoft.EntityFrameworkCore;
using System;
using TradeManagementApp.Persistence;
using TradeManagementApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;

public class DatabaseFixture : IDisposable
{
    private readonly string _dbName;
    private static bool _seeded = false; // Track if data has been seeded

    public DataContext Context { get; private set; }

    public DatabaseFixture()
    {
        _dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: _dbName)
            .Options;

        Context = new DataContext(options);
        Context.Database.EnsureCreated();

        EnsureSeeded();
    }

    private void EnsureSeeded()
    {
        if (!_seeded)
        {
            SeedData();
            _seeded = true;
        }
    }

    private void SeedData()
    {
        if (!Context.Trades.Any())
        {
            Context.Trades.AddRange(
                new Trade { Id = 1, Status = TradeStatus.Placed, BuyOrSell = "Buy", SecurityCode = "AAPL" },
                new Trade { Id = 2, Status = TradeStatus.Executed, BuyOrSell = "Sell", SecurityCode = "GOOGL" }
            );

            Context.Accounts.AddRange(
                new Account { Id = 1, FirstName = "John", LastName = "Doe" },
                new Account { Id = 2, FirstName = "Jane", LastName = "Doe" }
            );

            Context.SaveChanges();

            // Detach all entities after seeding
            foreach (var entry in Context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
    }

    public DataContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: _dbName)
            .Options;

        return new DataContext(options);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}