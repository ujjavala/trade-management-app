using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Persistence;
using TradeManagementApp.Persistence.Repositories;
using Xunit;

namespace TradeManagementApp.Tests.Repositories
{
    public class TradeRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public TradeRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private (TradeRepository repository, DataContext context) CreateRepository()
        {
            var context = _fixture.CreateContext();
            var repository = new TradeRepository(context);
            return (repository, context);
        }


        [Fact]
        public async Task GetTradeByIdAsync_WithValidId_ReturnsTrade()
        {
            // Arrange
            var (repository, context) = CreateRepository();

            // Act
            var result = await repository.GetTradeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetTradeByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var (repository, context) = CreateRepository();

            // Act
            var result = await repository.GetTradeByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateTradeAsync_InvalidTrade_ThrowsException()
        {
            // Arrange
            var (repository, context) = CreateRepository();
            var trade = new Trade { Id = 99, Status = TradeStatus.Executed, BuyOrSell = "Sell", SecurityCode = "AAPL" };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.UpdateTradeAsync(trade));
        }

        [Fact]
        public async Task DeleteTradeAsync_ValidId_DeletesTrade()
        {
            // Arrange
            var (repository, context) = CreateRepository();

            // Act
            await repository.DeleteTradeAsync(1);
            await context.SaveChangesAsync();

            var result = await repository.GetTradeByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTradeAsync_InvalidId_DoesNothing()
        {
            // Arrange
            var (repository, context) = CreateRepository();

            // Act
            await repository.DeleteTradeAsync(99);
            await context.SaveChangesAsync();

            var result = await repository.GetTradeByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
    }
}