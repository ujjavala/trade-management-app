// <copyright file="AccountServiceTests.cs" company="PlaceholderCompany">
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

    public class AccountServiceTests
    {
        private readonly IAccountService service;
        private readonly Mock<IAccountRepository> mockAccountRepository;

        public AccountServiceTests()
        {
            mockAccountRepository = new Mock<IAccountRepository>();
            service = new AccountService(mockAccountRepository.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsAccounts()
        {
            // Arrange
            mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync())
                .ReturnsAsync(new List<Account>());

            // Act
            var result = await service.GetAllAccountsAsync();

            // Assert
            Assert.IsType<List<Account>>(result);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ReturnsAccount()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, FirstName = "John", LastName = "Doe" };
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            var result = await service.GetAccountByIdAsync(accountId);

            // Assert
            Assert.IsType<Account>(result);
        }

        [Fact]
        public async Task AddAccountAsync_ReturnsAccount()
        {
            // Arrange
            var account = new Account { FirstName = "John", LastName = "Doe" };
            mockAccountRepository.Setup(repo => repo.AddAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            await service.AddAccountAsync(account);

            // Assert
            mockAccountRepository.Verify(repo => repo.AddAccountAsync(account), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountAsync_ReturnsTrue()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
            mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            await service.UpdateAccountAsync(account);

            // Assert
            mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(account), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountAsync_ReturnsTrue()
        {
            // Arrange
            var accountId = 1;
            mockAccountRepository.Setup(repo => repo.DeleteAccountAsync(accountId))
                .Returns(Task.CompletedTask);

            // Act
            await service.DeleteAccountAsync(accountId);

            // Assert
            mockAccountRepository.Verify(repo => repo.DeleteAccountAsync(accountId), Times.Once);
        }
    }
}
