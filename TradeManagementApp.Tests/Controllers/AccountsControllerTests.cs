using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.API.Controllers;
using TradeManagementApp.Application.Services;
using TradeManagementApp.Domain.Models;
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
        public async Task GetAllAccounts_ReturnsOkResult_WithAccounts()
        {
            // Arrange
            var accounts = new List<Account> {
                new Account { Id = 1, FirstName = "John", LastName = "Doe" },
                new Account { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };
            _mockAccountService.Setup(service => service.GetAllAccountsAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _controller.GetAccounts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccounts = Assert.IsType<List<Account>>(okResult.Value);
            Assert.Equal(2, returnedAccounts.Count);
        }

        [Fact]
        public async Task GetAllAccounts_ReturnsOkResult_WhenNoAccountsExist()
        {
            // Arrange
            _mockAccountService.Setup(service => service.GetAllAccountsAsync()).ReturnsAsync(new List<Account>());

            // Act
            var result = await _controller.GetAccounts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccounts = Assert.IsType<List<Account>>(okResult.Value);
            Assert.Empty(returnedAccounts);
        }

        [Fact]
        public async Task GetAccountById_ReturnsOkResult_WithAccount()
        {
            // Arrange
            var accountId = 1;
            var expectedAccount = new Account { Id = accountId, FirstName = "John", LastName = "Doe" };
            _mockAccountService.Setup(service => service.GetAccountByIdWithCacheAsync(accountId)).ReturnsAsync(expectedAccount);

            // Act
            var result = await _controller.GetAccount(accountId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccount = Assert.IsType<Account>(okResult.Value);
            Assert.Equal(accountId, returnedAccount.Id);
            Assert.Equal("John", returnedAccount.FirstName);
            Assert.Equal("Doe", returnedAccount.LastName);
        }

        [Fact]
        public async Task GetAccountById_ReturnsNotFoundResult_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            _mockAccountService.Setup(service => service.GetAccountByIdWithCacheAsync(accountId)).ReturnsAsync(null as Account);

            // Act
            var result = await _controller.GetAccount(accountId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreatedAtActionResult_WithAccount()
        {
            // Arrange
            var account = new Account { FirstName = "John", LastName = "Doe" };
            _mockAccountService.Setup(service => service.AddAccountAsync(account)).Returns(Task.CompletedTask);
            _mockAccountService.Setup(service => service.GetAccountByIdAsync(It.IsAny<int>())).ReturnsAsync(account);

            // Act
            var result = await _controller.PostAccount(account);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(account, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateAccount_ReturnsNoContentResult()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, FirstName = "UpdatedJohn", LastName = "UpdatedDoe" };
            _mockAccountService.Setup(service => service.UpdateAccountAsync(account)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutAccount(accountId, account);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAccount_ReturnsBadRequest_WhenAccountIdsDoNotMatch()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = 2, FirstName = "UpdatedJohn", LastName = "UpdatedDoe" };

            // Act
            var result = await _controller.PutAccount(accountId, account);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteAccount_ReturnsNoContentResult()
        {
            // Arrange
            var accountId = 1;
            _mockAccountService.Setup(service => service.DeleteAccountAsync(accountId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAccount(accountId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}