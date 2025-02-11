using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using TradeManagementApp.Application.Services;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Domain.Repositories;
using Xunit;

namespace TradeManagementApp.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly IAccountService _service;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IMemoryCache> _mockMemoryCache;

        public AccountServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMemoryCache = new Mock<IMemoryCache>(); // Initialize mockMemoryCache
            _service = new AccountService(_mockAccountRepository.Object, _mockMemoryCache.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsAccounts()
        {
            // Arrange
            var accounts = new List<Account> { new Account(), new Account() };
            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _service.GetAllAccountsAsync();

            // Assert
            Assert.Equal(accounts, result);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ReturnsAccount()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId };
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId)).ReturnsAsync(account);

            // Act
            var result = await _service.GetAccountByIdAsync(accountId);

            // Assert
            Assert.Equal(account, result);
        }

        [Fact]
        public async Task AddAccountAsync_ValidAccount_CallsRepository()
        {
            // Arrange
            var account = new Account { FirstName = "John", LastName = "Doe" };

            // Act
            await _service.AddAccountAsync(account);

            // Assert
            _mockAccountRepository.Verify(repo => repo.AddAccountAsync(account), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountAsync_ValidAccount_CallsRepository()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
            _mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(account)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAccountAsync(account);

            // Assert
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(account), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountAsync_ExistingAccountId_CallsRepository()
        {
            // Arrange
            var accountId = 1;

            // Act
            await _service.DeleteAccountAsync(accountId);

            // Assert
            _mockAccountRepository.Verify(repo => repo.DeleteAccountAsync(accountId), Times.Once);
        }

        [Fact]
        public async Task GetAllAccountsAsync_NoAccounts_ReturnsEmptyList()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync()).ReturnsAsync(new List<Account>());

            // Act
            var result = await _service.GetAllAccountsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAccountByIdAsync_NonExistingAccountId_ReturnsNull()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(1)).ReturnsAsync((Account)null);

            // Act
            var result = await _service.GetAccountByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAccountAsync_MissingFirstName_ThrowsArgumentException()
        {
            // Arrange
            var account = new Account { LastName = "Doe" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAccountAsync(account));
        }

        [Fact]
        public async Task AddAccountAsync_MissingLastName_ThrowsArgumentException()
        {
            // Arrange
            var account = new Account { FirstName = "John" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAccountAsync(account));
        }

        [Fact]
        public async Task UpdateAccountAsync_NonExistingAccount_ThrowsException()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
            _mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(account)).ThrowsAsync(new Exception("Account not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAccountAsync(account));
        }

        [Fact]
        public async Task DeleteAccountAsync_NonExistingAccount_ThrowsException()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.DeleteAccountAsync(1)).ThrowsAsync(new Exception("Account not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAccountAsync(1));
        }

        [Fact(Skip ="Fix this test")]
        public async Task GetAccountByIdWithCacheAsync_AccountInCache_ReturnsCachedAccount()
        {
            // Arrange
            var accountId = 1;
            var cachedAccount = new Account { Id = accountId, FirstName = "Cached", LastName = "Account" };
            var cacheKey = $"account-{accountId}";

            object cachedObject;
            _mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out cachedObject))
                .Returns(true)
                .Callback((string key, out object value) =>
                {
                    value = cachedAccount;
                });

            // Act
            var result = await _service.GetAccountByIdWithCacheAsync(accountId);

            // Assert
            Assert.Equal(cachedAccount, result);
            _mockAccountRepository.Verify(repo => repo.GetAccountByIdAsync(accountId), Times.Never); // Should not hit the repository
        }

        [Fact]
        public async Task GetAccountByIdWithCacheAsync_AccountNotInCache_ReturnsAccountFromRepositoryAndAddsToCache()
        {
            // Arrange
            var accountId = 1;
            var repositoryAccount = new Account { Id = accountId, FirstName = "Repository", LastName = "Account" };
            var cacheKey = $"account-{accountId}";

            _mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out It.Ref<object>.IsAny)).Returns(false);
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId)).ReturnsAsync(repositoryAccount);
            _mockMemoryCache.Setup(cache => cache.CreateEntry(cacheKey)).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _service.GetAccountByIdWithCacheAsync(accountId);

            // Assert
            Assert.Equal(repositoryAccount, result);
            _mockAccountRepository.Verify(repo => repo.GetAccountByIdAsync(accountId), Times.Once); // Should hit the repository
            _mockMemoryCache.Verify(cache => cache.CreateEntry(cacheKey), Times.Once); // Should add to cache
        }

        [Fact]
        public async Task GetAccountByIdWithCacheAsync_AccountNotInCacheAndRepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var accountId = 1;
            var cacheKey = $"account-{accountId}";

            _mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out It.Ref<object>.IsAny)).Returns(false);
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId)).ReturnsAsync((Account)null);

            // Act
            var result = await _service.GetAccountByIdWithCacheAsync(accountId);

            // Assert
            Assert.Null(result);
            _mockAccountRepository.Verify(repo => repo.GetAccountByIdAsync(accountId), Times.Once); // Should hit the repository
            _mockMemoryCache.Verify(cache => cache.CreateEntry(cacheKey), Times.Never); // Should not add to cache
        }

        [Fact(Skip ="Fix this test")]
        public async Task GetAccountByIdWithCacheAsync_CacheThrowsException_ReturnsAccountFromRepository()
        {
            // Arrange
            var accountId = 1;
            var repositoryAccount = new Account { Id = accountId, FirstName = "Repository", LastName = "Account" };
            var cacheKey = $"account-{accountId}";

            _mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out It.Ref<object>.IsAny)).Throws(new Exception("Cache unavailable"));
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId)).ReturnsAsync(repositoryAccount);

            // Act
            var result = await _service.GetAccountByIdWithCacheAsync(accountId);

            // Assert
            Assert.Equal(repositoryAccount, result);
            _mockAccountRepository.Verify(repo => repo.GetAccountByIdAsync(accountId), Times.Once); // Should hit the repository
        }

        [Fact]
        public async Task GetAccountByIdWithCacheAsync_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var accountId = 1;
            var cacheKey = $"account-{accountId}";

            _mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out It.Ref<object>.IsAny)).Returns(false);
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId)).ThrowsAsync(new Exception("Repository unavailable"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAccountByIdWithCacheAsync(accountId));
            _mockAccountRepository.Verify(repo => repo.GetAccountByIdAsync(accountId), Times.Once); // Should try to hit the repository
        }
    }
}