using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TradeManagementApp.API.Controllers;
using TradeManagementApp.Application.Services;
using TradeManagementApp.Domain.Models;
using Xunit;

namespace TradeManagementApp.Tests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountsController _controller;

        public AccountsControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountsController(_mockAccountService.Object);
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
        public async Task UpdateAccount_ReturnsBadRequest_WhenAccountIdsDoNotMatch()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _controller.PutAccount(2, account);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("The ID in the URL does not match the ID in the body of the request.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAccount_UsesIdFromUrl_WhenNoIdInBody()
        {
            // Arrange
            var account = new Account { FirstName = "John", LastName = "Doe" };
            _mockAccountService.Setup(service => service.UpdateAccountAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);
            _mockAccountService.Setup(service => service.GetAccountByIdAsync(It.IsAny<int>())).ReturnsAsync(account);

            // Act
            var result = await _controller.PutAccount(1, account);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccount = Assert.IsType<Account>(okResult.Value);
            Assert.Equal(1, returnedAccount.Id);
        }

        [Fact]
        public async Task UpdateAccount_ReturnsUpdatedAccount()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
            _mockAccountService.Setup(service => service.UpdateAccountAsync(account)).Returns(Task.CompletedTask);
            _mockAccountService.Setup(service => service.GetAccountByIdAsync(account.Id)).ReturnsAsync(account);

            // Act
            var result = await _controller.PutAccount(1, account);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccount = Assert.IsType<Account>(okResult.Value);
            Assert.Equal(account.Id, returnedAccount.Id);
            Assert.Equal(account.FirstName, returnedAccount.FirstName);
            Assert.Equal(account.LastName, returnedAccount.LastName);
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