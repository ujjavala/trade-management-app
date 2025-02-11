using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace TradeManagementApp.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly LruCache<string, Account> _lruCache;

        public AccountService(IAccountRepository accountRepository, LruCache<string, Account> lruCache)
        {
            _accountRepository = accountRepository;
            _lruCache = lruCache;
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

            var account = _lruCache.Get(cacheKey);
            if (account != null)
            {
                return account; // Return cached account
            }

            account = await _accountRepository.GetAccountByIdAsync(accountId);

            if (account != null)
            {
                _lruCache.Set(cacheKey, account);
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