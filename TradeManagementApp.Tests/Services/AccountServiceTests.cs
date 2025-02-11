// <copyright file="AccountServiceTests.cs" company="PlaceholderCompany">
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
            Assert.Equal(accountId, result.Id);
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

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsEmptyList_WhenNoAccountsExist()
        {
            // Arrange
            mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync())
                .ReturnsAsync(new List<Account>());

            // Act
            var result = await service.GetAllAccountsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ReturnsNull_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId))
                .ReturnsAsync((Account)null);

            // Act
            var result = await service.GetAccountByIdAsync(accountId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAccountAsync_ThrowsException_WhenFirstNameIsMissing()
        {
            // Arrange
            var account = new Account { LastName = "Doe" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAccountAsync(account));
        }

        [Fact]
        public async Task AddAccountAsync_ThrowsException_WhenLastNameIsMissing()
        {
            // Arrange
            var account = new Account { FirstName = "John" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAccountAsync(account));
        }

        [Fact]
        public async Task UpdateAccountAsync_ThrowsException_WhenAccountNotFound()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
            mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(account))
                .ThrowsAsync(new Exception("Account not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.UpdateAccountAsync(account));
        }

        [Fact]
        public async Task DeleteAccountAsync_ThrowsException_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            mockAccountRepository.Setup(repo => repo.DeleteAccountAsync(accountId))
                .ThrowsAsync(new Exception("Account not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.DeleteAccountAsync(accountId));
        }
    }
}