using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TradeManagementApp.API.Services;
using TradeManagementApp.Models;
using TradeManagementApp.Persistence.Repositories;
using Xunit;

namespace TradeManagementApp.Tests.Services
{
    public class TradeServiceTests
    {
        private readonly ITradeService _service;
        private readonly Mock<ITradeRepository> _mockTradeRepository;
        private readonly Mock<IAccountRepository> _mockAccountRepository;

        public TradeServiceTests()
        {
            _mockTradeRepository = new Mock<ITradeRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _service = new TradeService(_mockTradeRepository.Object, _mockAccountRepository.Object);
        }

        [Fact]
        public async Task GetAllTradesAsync_ReturnsTrades()
        {
            // Arrange
            _mockTradeRepository.Setup(repo => repo.GetAllTradesAsync())
                .ReturnsAsync(new List<Trade>());

            // Act
            var result = await _service.GetAllTradesAsync();

            // Assert
            Assert.IsType<List<Trade>>(result);
        }

        [Fact]
        public async Task GetTradeByIdAsync_ReturnsTrade()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, SecurityCode = "AAPL", Amount = 100 };
            _mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);

            // Act
            var result = await _service.GetTradeByIdAsync(tradeId);

            // Assert
            Assert.IsType<Trade>(result);
        }

        [Fact]
        public async Task AddTradeAsync_AddsTrade()
        {
            // Arrange
            var trade = new Trade { SecurityCode = "AAPL", Amount = 100, AccountId = 1 };
            _mockTradeRepository.Setup(repo => repo.AddTradeAsync(trade))
                .Returns(Task.CompletedTask);
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync(new Account { Id = trade.AccountId });

            // Act
            await _service.AddTradeAsync(trade);

            // Assert
            _mockTradeRepository.Verify(repo => repo.AddTradeAsync(trade), Times.Once);
        }

        [Fact]
        public async Task UpdateTradeAsync_UpdatesTrade()
        {
            // Arrange
            var trade = new Trade { Id = 1, SecurityCode = "AAPL", Amount = 100, AccountId = 1 };
            _mockTradeRepository.Setup(repo => repo.UpdateTradeAsync(trade))
                .Returns(Task.CompletedTask);
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync(new Account { Id = trade.AccountId });

            // Act
            await _service.UpdateTradeAsync(trade);

            // Assert
            _mockTradeRepository.Verify(repo => repo.UpdateTradeAsync(trade), Times.Once);
        }

        [Fact]
        public async Task DeleteTradeAsync_DeletesTrade()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, AccountId = 1 };
            _mockTradeRepository.Setup(repo => repo.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);
            _mockTradeRepository.Setup(repo => repo.DeleteTradeAsync(tradeId))
                .Returns(Task.CompletedTask);
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(trade.AccountId))
                .ReturnsAsync(new Account { Id = trade.AccountId });

            // Act
            await _service.DeleteTradeAsync(tradeId);

            // Assert
            _mockTradeRepository.Verify(repo => repo.DeleteTradeAsync(tradeId), Times.Once);
        }
    }
}