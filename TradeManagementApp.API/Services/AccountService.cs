// <copyright file="AccountService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TradeManagementApp.Models;
    using TradeManagementApp.Persistence.Repositories;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAccountsAsync();
        }

        /// <inheritdoc/>
        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _accountRepository.GetAccountByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddAccountAsync(Account account)
        {
            if (string.IsNullOrEmpty(account.FirstName))
            {
                throw new ArgumentException("First Name is required");
            }

            if (string.IsNullOrEmpty(account.LastName))
            {
                throw new ArgumentException("Last Name is required");
            }

            await _accountRepository.AddAccountAsync(account);
        }

        /// <inheritdoc/>
        public async Task UpdateAccountAsync(Account account)
        {
            await _accountRepository.UpdateAccountAsync(account);
        }

        /// <inheritdoc/>
        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAccountAsync(id);
        }
    }
}