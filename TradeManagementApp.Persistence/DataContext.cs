using Microsoft.EntityFrameworkCore;
using TradeManagementApp.Models;
using System;

namespace TradeManagementApp.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Trade> Trades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trade>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trade>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Trades)
                .HasForeignKey(t => t.AccountId);

            // Seed data for Accounts
            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, FirstName = "John", LastName = "Doe" },
                new Account { Id = 2, FirstName = "Jane", LastName = "Smith" }
            );

            // Seed data for Trades
            modelBuilder.Entity<Trade>().HasData(
                new Trade { Id = 1, AccountId = 1, SecurityCode = "AAP", Timestamp = DateTime.Now, Amount = 100, BuyOrSell = "Buy", Status = TradeStatus.Executed },
                new Trade { Id = 2, AccountId = 2, SecurityCode = "OGL", Timestamp = DateTime.Now, Amount = 200, BuyOrSell = "Sell", Status = TradeStatus.Executed }
            );
        }
    }
}