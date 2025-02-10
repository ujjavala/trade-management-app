using System;
using Microsoft.EntityFrameworkCore;
using TradeManagementApp.Models;

namespace TradeManagementApp.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Trade> Trades { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Accounts
            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, FirstName = "John", LastName = "Doe" },
                new Account { Id = 2, FirstName = "Jane", LastName = "Smith" }
            );

            // Seed data for Trades
            modelBuilder.Entity<Trade>().HasData(
                new Trade { Id = 1, AccountId = 1, SecurityCode = "AAPL", Timestamp = DateTime.Now, Amount = 100, BuyOrSell = "Buy", Status = "Completed" },
                new Trade { Id = 2, AccountId = 2, SecurityCode = "GOOGL", Timestamp = DateTime.Now, Amount = 200, BuyOrSell = "Sell", Status = "Completed" }
            );
        }
    }
}