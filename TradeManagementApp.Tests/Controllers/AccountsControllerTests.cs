// <copyright file="AccountsControllerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using TradeManagementApp.API.Controllers;
    using TradeManagementApp.API.Services;
    using TradeManagementApp.Models;
    using Xunit;


    public class AccountsControllerTests
    {
        private readonly AccountsController controller;
        private readonly Mock<IAccountService> mockAccountService;

        public AccountsControllerTests()
        {
            mockAccountService = new Mock<IAccountService>();
            controller = new AccountsController(mockAccountService.Object);
        }

        [Fact]
        public async Task GetAllAccounts_ReturnsOkResult()
        {
            // Arrange
            mockAccountService.Setup(service => service.GetAllAccountsAsync())
                .ReturnsAsync(new List<Account>());

            // Act
            var result = await controller.GetAllAccounts();

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
            mockAccountService.Setup(service => service.GetAccountByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            var result = await controller.GetAccountById(accountId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Account>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var account = new Account { FirstName = "John", LastName = "Doe" };
            mockAccountService.Setup(service => service.AddAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.CreateAccount(account);

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
            mockAccountService.Setup(service => service.UpdateAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateAccount(accountId, account);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAccount_ReturnsNoContentResult()
        {
            // Arrange
            var accountId = 1;
            mockAccountService.Setup(service => service.DeleteAccountAsync(accountId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteAccount(accountId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
