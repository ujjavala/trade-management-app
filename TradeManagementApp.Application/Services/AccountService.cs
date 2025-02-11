using TradeManagementApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using TradeManagementApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task AddAccountAsync(Account account)
        {
            if (string.IsNullOrEmpty(account.FirstName))
            {
                throw new ArgumentException("FirstName is required.");
            }

            if (string.IsNullOrEmpty(account.LastName))
            {
                throw new ArgumentException("LastName is required.");
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

        public async Task<Account> GetAccountByIdWithCacheAsync(int id)
        {
            string cacheKey = $"account-{id}";

            // Try to get the account from the cache
            if (!_memoryCache.TryGetValue(cacheKey, out Account account))
            {
                // If not found in cache, fetch from the repository
                account = await _accountRepository.GetAccountByIdAsync(id);

                if (account != null)
                {
                    // Set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30)) // Expire if not accessed for 30 seconds
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(300)) // Expire after 5 minutes
                        .SetPriority(CacheItemPriority.Normal); // Keep in cache if memory is low

                    // Save to cache
                    _memoryCache.Set(cacheKey, account, cacheEntryOptions);
                }
            }

            return account;
        }
    }
}