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
    public class AccountsControllerTests
    {
        private readonly AccountsController _controller;
        private readonly Mock<IAccountService> _mockAccountService;

        public AccountsControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountsController(_mockAccountService.Object);
        }

        [Fact]
        public async Task GetAllAccounts_ReturnsOkResult()
        {
            // Arrange
            _mockAccountService.Setup(service => service.GetAllAccountsAsync())
                .ReturnsAsync(new List<Account>());

            // Act
            var result = await _controller.GetAllAccounts();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<Account>>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetAccountById_ReturnsOkResult()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, FirstName = "John", LastName = "Doe" };
            _mockAccountService.Setup(service => service.GetAccountByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            var result = await _controller.GetAccountById(accountId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Account>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var account = new Account { FirstName = "John", LastName = "Doe" };
            _mockAccountService.Setup(service => service.AddAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateAccount(account);

            // Assert
            var createdResult = Assert.IsType<ActionResult<Account>>(result);
            Assert.IsType<CreatedAtActionResult>(createdResult.Result);
        }

        [Fact]
        public async Task UpdateAccount_ReturnsNoContentResult()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, FirstName = "John", LastName = "Doe" };
            _mockAccountService.Setup(service => service.UpdateAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAccount(accountId, account);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAccount_ReturnsNoContentResult()
        {
            // Arrange
            var accountId = 1;
            _mockAccountService.Setup(service => service.DeleteAccountAsync(accountId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAccount(accountId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}