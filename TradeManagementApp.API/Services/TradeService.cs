// <copyright file="TradeService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.API.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TradeManagementApp.Models;
    using TradeManagementApp.Persistence.Repositories;

    public class TradeService : ITradeService
    {
        private readonly ITradeRepository tradeRepository;
        private readonly IAccountRepository accountRepository;

        public TradeService(ITradeRepository tradeRepository, IAccountRepository accountRepository)
        {
            this.tradeRepository = tradeRepository;
            this.accountRepository = accountRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            return await tradeRepository.GetAllTradesAsync();
        }

        /// <inheritdoc/>
        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await tradeRepository.GetTradeByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddTradeAsync(Trade trade)
        {
            if (await accountRepository.GetAccountByIdAsync(trade.AccountId) != null)
            {
                await tradeRepository.AddTradeAsync(trade);
            }
            else
            {
                throw new KeyNotFoundException("Account ID does not exist.");
            }
        }

        /// <inheritdoc/>
        public async Task UpdateTradeAsync(Trade trade)
        {
            if (await accountRepository.GetAccountByIdAsync(trade.AccountId) != null)
            {
                await tradeRepository.UpdateTradeAsync(trade);
            }
            else
            {
                throw new KeyNotFoundException("Account ID does not exist.");
            }
        }

        /// <inheritdoc/>
        public async Task DeleteTradeAsync(int id)
        {
            var trade = await tradeRepository.GetTradeByIdAsync(id);
            if (trade != null)
            {
                if (await accountRepository.GetAccountByIdAsync(trade.AccountId) != null)
                {
                    await tradeRepository.DeleteTradeAsync(id);
                }
                else
                {
                    throw new KeyNotFoundException("Account ID does not exist.");
                }
            }
        }
    }
}
