// <copyright file="IAccountService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;

namespace TradeManagementApp.Application.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();

        Task<Account> GetAccountByIdAsync(int id);

        Task AddAccountAsync(Account account);

        Task UpdateAccountAsync(Account account);

        Task DeleteAccountAsync(int id);
    }
}
