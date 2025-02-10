using Microsoft.EntityFrameworkCore;
using TradeManagementApp.Models;
using System;

namespace TradeManagementApp.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Trade> Trades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=trade_management.db"); // Use your actual database provider and connection string
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trade>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trade>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Trades)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data for Accounts
            for (int i = 1; i <= 50; i++)
            {
                modelBuilder.Entity<Account>().HasData(new Account
                {
                    Id = i,
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}"
                });
            }

            // Seed data for Trades
            for (int i = 1; i <= 50; i++)
            {
                modelBuilder.Entity<Trade>().HasData(new Trade
                {
                    Id = i,
                    AccountId = i,
                    Amount = i * 10m,
                    BuyOrSell = i % 2 == 0 ? "Buy" : "Sell",
                    SecurityCode = $"{i:D3}",
                    Status = TradeStatus.Placed,
                    Timestamp = DateTime.UtcNow.AddMinutes(i)
                });
            }
        }
    }
}