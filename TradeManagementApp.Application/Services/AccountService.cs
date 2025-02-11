using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory; // Add this using directive


namespace TradeManagementApp.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMemoryCache _memoryCache;

        public AccountService(IAccountRepository accountRepository, IMemoryCache memoryCache)
        {
            _accountRepository = accountRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAccountsAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _accountRepository.GetAccountByIdAsync(id);
        }

        public async Task<Account> GetAccountByIdWithCacheAsync(int accountId)
        {
            string cacheKey = $"account-{accountId}";

            if (_memoryCache.TryGetValue(cacheKey, out Account account))
            {
                return account; // Return cached account
            }

            account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account != null)
            {
                _memoryCache.Set(cacheKey, account, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
            }

            return account;
        }

        public async Task AddAccountAsync(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.FirstName))
            {
                throw new ArgumentException("First Name is required", nameof(account.FirstName));
            }

            if (string.IsNullOrWhiteSpace(account.LastName))
            {
                throw new ArgumentException("Last Name is required", nameof(account.LastName));
            }

            await _accountRepository.AddAccountAsync(account);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAccountAsync(id);
        }

        public async Task<IEnumerable<Account>> SearchAccountsAsync(int? id, string? lastName)
        {
            return await _accountRepository.SearchAccountsAsync(id, lastName);
        }
    }
}