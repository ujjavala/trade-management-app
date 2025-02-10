// <copyright file="AccountService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.API.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TradeManagementApp.Models;
    using TradeManagementApp.Persistence.Repositories;

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await accountRepository.GetAllAccountsAsync();
        }

        /// <inheritdoc/>
        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await accountRepository.GetAccountByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddAccountAsync(Account account)
        {
            await accountRepository.AddAccountAsync(account);
        }

        /// <inheritdoc/>
        public async Task UpdateAccountAsync(Account account)
        {
            await accountRepository.UpdateAccountAsync(account);
        }

        /// <inheritdoc/>
        public async Task DeleteAccountAsync(int id)
        {
            await accountRepository.DeleteAccountAsync(id);
        }
    }
}
