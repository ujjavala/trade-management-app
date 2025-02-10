using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TradeManagementApp.API.Services;
using TradeManagementApp.Models;
using TradeManagementApp.Persistence.Repositories;
using Xunit;

namespace TradeManagementApp.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly IAccountService _service;
        private readonly Mock<IAccountRepository> _mockAccountRepository;

        public AccountServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _service = new AccountService(_mockAccountRepository.Object);
        }

        [Fact]
        public async Task GetAllAccountsAsync_ReturnsAccounts()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.GetAllAccountsAsync())
                .ReturnsAsync(new List<Account>());

            // Act
            var result = await _service.GetAllAccountsAsync();

            // Assert
            Assert.IsType<List<Account>>(result);
        }

        [Fact]
        public async Task GetAccountByIdAsync_ReturnsAccount()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, FirstName = "John", LastName = "Doe" };
            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(accountId))
                .ReturnsAsync(account);

            // Act
            var result = await _service.GetAccountByIdAsync(accountId);

            // Assert
            Assert.IsType<Account>(result);
        }

        [Fact]
        public async Task AddAccountAsync_ReturnsAccount()
        {
            // Arrange
            var account = new Account { FirstName = "John", LastName = "Doe" };
            _mockAccountRepository.Setup(repo => repo.AddAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddAccountAsync(account);

            // Assert
            _mockAccountRepository.Verify(repo => repo.AddAccountAsync(account), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountAsync_ReturnsTrue()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
            _mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAccountAsync(account);

            // Assert
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(account), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountAsync_ReturnsTrue()
        {
            // Arrange
            var accountId = 1;
            _mockAccountRepository.Setup(repo => repo.DeleteAccountAsync(accountId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAccountAsync(accountId);

            // Assert
            _mockAccountRepository.Verify(repo => repo.DeleteAccountAsync(accountId), Times.Once);
        }
    }
}