using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TradeManagementApp.API.Controllers;
using TradeManagementApp.API.Services;
using TradeManagementApp.Models;
using Xunit;

namespace TradeManagementApp.Tests.Controllers
{
    public class TradesControllerTests
    {
        private readonly TradesController _controller;
        private readonly Mock<ITradeService> _mockTradeService;

        public TradesControllerTests()
        {
            _mockTradeService = new Mock<ITradeService>();
            _controller = new TradesController(_mockTradeService.Object);
        }

        [Fact]
        public async Task GetAllTrades_ReturnsOkResult()
        {
            // Arrange
            _mockTradeService.Setup(service => service.GetAllTradesAsync())
                .ReturnsAsync(new List<Trade>());

            // Act
            var result = await _controller.GetAllTrades();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<Trade>>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetTradeById_ReturnsOkResult()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, SecurityCode = "AAPL", Amount = 100 };
            _mockTradeService.Setup(service => service.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);

            // Act
            var result = await _controller.GetTradeById(tradeId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Trade>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task CreateTrade_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var trade = new Trade { SecurityCode = "AAPL", Amount = 100 };
            _mockTradeService.Setup(service => service.AddTradeAsync(trade))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateTrade(trade);

            // Assert
            var createdResult = Assert.IsType<ActionResult<Trade>>(result);
            Assert.IsType<CreatedAtActionResult>(createdResult.Result);
        }

        [Fact]
        public async Task UpdateTrade_ReturnsNoContentResult()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, SecurityCode = "AAPL", Amount = 100 };
            _mockTradeService.Setup(service => service.UpdateTradeAsync(trade))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTrade(tradeId, trade);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTrade_ReturnsNoContentResult()
        {
            // Arrange
            var tradeId = 1;
            _mockTradeService.Setup(service => service.DeleteTradeAsync(tradeId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTrade(tradeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}