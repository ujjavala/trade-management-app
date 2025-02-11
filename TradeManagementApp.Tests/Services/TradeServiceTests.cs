// <copyright file="TradeServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using TradeManagementApp.Application.Services;
    using TradeManagementApp.Domain.Models;
    using TradeManagementApp.Domain.Repositories;
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
            var trade = new Trade { Id = tradeId, SecurityCode = "APL", Amount = 100 };
            mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);

            // Act
            var result = await service.GetTradeByIdAsync(tradeId);

            // Assert
            Assert.IsType<Trade>(result);
            Assert.Equal(tradeId, result.Id);
        }

        [Fact]
        public async Task GetTradeByIdAsync_ReturnsNull_WhenTradeNotFound()
        {
            // Arrange
            var tradeId = 1;
            mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync((Trade)null);

            // Act
            var result = await service.GetTradeByIdAsync(tradeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddTradeAsync_AddsTrade()
        {
            // Arrange
            var trade = new Trade { SecurityCode = "APL", Amount = 100, AccountId = 1 };
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
        public async Task AddTradeAsync_ThrowsException_WhenAccountNotFound()
        {
            // Arrange
            var trade = new Trade { SecurityCode = "APL", Amount = 100, AccountId = 1 };
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AddTradeAsync(trade));
        }

        [Fact]
        public async Task AddTradeAsync_ThrowsException_WhenSecurityCodeIsInvalid()
        {
            // Arrange
            var trade1 = new Trade { SecurityCode = "AP", Amount = 100, AccountId = 1 }; // Less than 3 characters
            var trade2 = new Trade { SecurityCode = "APPL", Amount = 100, AccountId = 1 }; // More than 3 characters
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade1.AccountId))
                .ReturnsAsync(new Account { Id = trade1.AccountId });
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade2.AccountId))
                .ReturnsAsync(new Account { Id = trade2.AccountId });

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddTradeAsync(trade1));
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddTradeAsync(trade2));
        }

        [Fact]
        public async Task UpdateTradeAsync_UpdatesTrade()
        {
            // Arrange
            var trade = new Trade { Id = 1, SecurityCode = "APL", Amount = 100, AccountId = 1 };
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
        public async Task UpdateTradeAsync_ThrowsException_WhenAccountNotFound()
        {
            // Arrange
            var trade = new Trade { Id = 1, SecurityCode = "APL", Amount = 100, AccountId = 1 };
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateTradeAsync(trade));
        }

        [Fact]
        public async Task UpdateTradeAsync_ThrowsException_WhenSecurityCodeIsInvalid()
        {
            // Arrange
            var trade1 = new Trade { Id = 1, SecurityCode = "AP", Amount = 100, AccountId = 1 }; // Less than 3 characters
            var trade2 = new Trade { Id = 1, SecurityCode = "APPL", Amount = 100, AccountId = 1 }; // More than 3 characters
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade1.AccountId))
                .ReturnsAsync(new Account { Id = trade1.AccountId });
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade2.AccountId))
                .ReturnsAsync(new Account { Id = trade2.AccountId });

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateTradeAsync(trade1));
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateTradeAsync(trade2));
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

        [Fact]
        public async Task DeleteTradeAsync_ThrowsException_WhenTradeNotFound()
        {
            // Arrange
            var tradeId = 1;
            mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync((Trade)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteTradeAsync(tradeId));
        }

        [Fact]
        public async Task DeleteTradeAsync_ThrowsException_WhenAccountNotFound()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, AccountId = 1 };
            mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteTradeAsync(tradeId));
        }
    }
}