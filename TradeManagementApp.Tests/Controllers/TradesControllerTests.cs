using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TradeManagementApp.API.Controllers;
using TradeManagementApp.Application.Services;
using TradeManagementApp.Domain.Models;
using Xunit;

namespace TradeManagementApp.Tests.Controllers
{
    public class TradesControllerTests
    {
        private readonly TradesController controller;
        private readonly Mock<ITradeService> mockTradeService;

        public TradesControllerTests()
        {
            mockTradeService = new Mock<ITradeService>();
            controller = new TradesController(mockTradeService.Object);
        }

        [Fact]
        public async Task GetAllTrades_ReturnsOkResult()
        {
            // Arrange
            mockTradeService.Setup(service => service.GetAllTradesAsync())
                .ReturnsAsync(new List<Trade>());

            // Act
            var result = await controller.GetAllTrades();

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
            mockTradeService.Setup(service => service.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);

            // Act
            var result = await controller.GetTradeById(tradeId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Trade>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task CreateTrade_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var trade = new Trade { SecurityCode = "AAPL", Amount = 100 };
            mockTradeService.Setup(service => service.AddTradeAsync(trade))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.CreateTrade(trade);

            // Assert
            var createdResult = Assert.IsType<ActionResult<Trade>>(result);
            Assert.IsType<CreatedAtActionResult>(createdResult.Result);
        }

        [Fact]
        public async Task UpdateTrade_ReturnsBadRequest_WhenTradeIdsDoNotMatch()
        {
            // Arrange
            var trade = new Trade { Id = 1, SecurityCode = "AAPL", Amount = 100 };

            // Act
            var result = await controller.UpdateTrade(2, trade);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("The ID in the URL does not match the ID in the body of the request.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateTrade_UsesIdFromUrl_WhenNoIdInBody()
        {
            // Arrange
            var trade = new Trade { SecurityCode = "AAPL", Amount = 100 };
            mockTradeService.Setup(service => service.UpdateTradeAsync(It.IsAny<Trade>())).Returns(Task.CompletedTask);
            mockTradeService.Setup(service => service.GetTradeByIdAsync(It.IsAny<int>())).ReturnsAsync(trade);

            // Act
            var result = await controller.UpdateTrade(1, trade);

            // Assert
            var okResult = Assert.IsType<ActionResult<Trade>>(result);
            var returnedTrade = Assert.IsType<OkObjectResult>(okResult.Result).Value as Trade;
            Assert.NotNull(returnedTrade);
            Assert.Equal(1, returnedTrade.Id);
        }

        [Fact]
        public async Task UpdateTrade_ReturnsUpdatedTrade()
        {
            // Arrange
            var tradeId = 1;
            var trade = new Trade { Id = tradeId, SecurityCode = "AAPL", Amount = 100 };
            mockTradeService.Setup(service => service.UpdateTradeAsync(trade))
                .Returns(Task.CompletedTask);
            mockTradeService.Setup(service => service.GetTradeByIdAsync(tradeId))
                .ReturnsAsync(trade);

            // Act
            var result = await controller.UpdateTrade(tradeId, trade);

            // Assert
            var okResult = Assert.IsType<ActionResult<Trade>>(result);
            var returnedTrade = Assert.IsType<OkObjectResult>(okResult.Result).Value as Trade;
            Assert.NotNull(returnedTrade);
            Assert.Equal(tradeId, returnedTrade.Id);
            Assert.Equal(trade.SecurityCode, returnedTrade.SecurityCode);
            Assert.Equal(trade.Amount, returnedTrade.Amount);
        }

        [Fact]
        public async Task DeleteTrade_ReturnsNoContentResult()
        {
            // Arrange
            var tradeId = 1;
            mockTradeService.Setup(service => service.DeleteTradeAsync(tradeId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteTrade(tradeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}