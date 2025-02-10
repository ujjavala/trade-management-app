// <copyright file="TradeServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using TradeManagementApp.API.Services;
    using TradeManagementApp.Models;
    using TradeManagementApp.Persistence.Repositories;
    using Xunit;

    public class TradeServiceTests
    {
        private readonly ITradeService service;
        private readonly Mock<ITradeRepository> mockTradeRepository;
        private readonly Mock<IAccountRepository> mockAccountRepository;

        public TradeServiceTests()
        {
            mockTradeRepository = new Mock<ITradeRepository>();
            mockAccountRepository = new Mock<IAccountRepository>();
            service = new TradeService(mockTradeRepository.Object, mockAccountRepository.Object);
        }

        [Fact]
        public async Task GetAllTradesAsync_ReturnsTrades()
        {
            // Arrange
            mockTradeRepository.Setup(repo => repo.GetAllTradesAsync())
                .ReturnsAsync(new List<Trade>());

            // Act
            var result = await service.GetAllTradesAsync();

            // Assert
            Assert.IsType<List<Trade>>(result);
        }

        [Fact]
        public async Task GetTradeByIdAsync_ReturnsTrade()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, SecurityCode = "AAPL", Amount = 100 };
            mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);

            // Act
            var result = await service.GetTradeByIdAsync(tradeId);

            // Assert
            Assert.IsType<Trade>(result);
        }

        [Fact]
        public async Task AddTradeAsync_AddsTrade()
        {
            // Arrange
            var trade = new Trade { SecurityCode = "AAPL", Amount = 100, AccountId = 1 };
            mockTradeRepository.Setup(repo => repo.AddTradeAsync(trade))
                .Returns(Task.CompletedTask);
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync(new Account { Id = trade.AccountId });

            // Act
            await service.AddTradeAsync(trade);

            // Assert
            mockTradeRepository.Verify(repo => repo.AddTradeAsync(trade), Times.Once);
        }

        [Fact]
        public async Task UpdateTradeAsync_UpdatesTrade()
        {
            // Arrange
            var trade = new Trade { Id = 1, SecurityCode = "AAPL", Amount = 100, AccountId = 1 };
            mockTradeRepository.Setup(repo => repo.UpdateTradeAsync(trade))
                .Returns(Task.CompletedTask);
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync(new Account { Id = trade.AccountId });

            // Act
            await service.UpdateTradeAsync(trade);

            // Assert
            mockTradeRepository.Verify(repo => repo.UpdateTradeAsync(trade), Times.Once);
        }

        [Fact]
        public async Task DeleteTradeAsync_DeletesTrade()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, AccountId = 1 };
            mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);
            mockTradeRepository.Setup(repo => repo.DeleteTradeAsync(tradeId))
                .Returns(Task.CompletedTask);
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync(new Account { Id = trade.AccountId });

            // Act
            await service.DeleteTradeAsync(tradeId);

            // Assert
            mockTradeRepository.Verify(repo => repo.DeleteTradeAsync(tradeId), Times.Once);
        }
    }
}
