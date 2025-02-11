using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Persistence;
using TradeManagementApp.Persistence.Repositories;
using Xunit;

namespace TradeManagementApp.Tests.Repositories
{
    public class AccountRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DataContext _context;
        private readonly AccountRepository _accountRepository;

        public AccountRepositoryTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            _accountRepository = new AccountRepository(_context);
        }

        [Fact]
        public async Task SearchAccountsAsync_WithValidId_ReturnsAccounts()
        {
            // Act
            var result = await _accountRepository.SearchAccountsAsync(1, null);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task SearchAccountsAsync_WithValidLastName_ReturnsAccounts()
        {
            // Act
            var result = await _accountRepository.SearchAccountsAsync(null, "Doe");

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task SearchAccountsAsync_WithInvalidId_ReturnsEmpty()
        {
            // Act
            var result = await _accountRepository.SearchAccountsAsync(99, null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchAccountsAsync_WithInvalidLastName_ReturnsEmpty()
        {
            // Act
            var result = await _accountRepository.SearchAccountsAsync(null, "NonExistent");

            // Assert
            Assert.Empty(result);
        }
    }
}