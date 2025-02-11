using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Domain.Repositories;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace TradeManagementApp.Application.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IAccountRepository _accountRepository;

        public TradeService(ITradeRepository tradeRepository, IAccountRepository accountRepository)
        {
            _tradeRepository = tradeRepository;
            _accountRepository = accountRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            return await _tradeRepository.GetAllTradesAsync();
        }

        /// <inheritdoc/>
        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _tradeRepository.GetTradeByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddTradeAsync(Trade trade)
        {
            var account = await _accountRepository.GetAccountByIdAsync(trade.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account ID does not exist.");
            }

            if (string.IsNullOrEmpty(trade.SecurityCode) || trade.SecurityCode.Length != 3)
            {
                throw new ArgumentException("Security code must be exactly 3 characters.");
            }

            await _tradeRepository.AddTradeAsync(trade);
        }

        /// <inheritdoc/>
        public async Task UpdateTradeAsync(Trade trade)
        {
            var account = await _accountRepository.GetAccountByIdAsync(trade.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account ID does not exist.");
            }

            if (string.IsNullOrEmpty(trade.SecurityCode) || trade.SecurityCode.Length != 3)
            {
                throw new ArgumentException("Security code must be exactly 3 characters.");
            }

            await _tradeRepository.UpdateTradeAsync(trade);
        }

        /// <inheritdoc/>
        public async Task DeleteTradeAsync(int id)
        {
            var trade = await _tradeRepository.GetTradeByIdAsync(id);
            if (trade == null)
            {
                throw new ArgumentException("Trade ID not found.");
            }

            var account = await _accountRepository.GetAccountByIdAsync(trade.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account ID does not exist.");
            }

            await _tradeRepository.DeleteTradeAsync(id);
        }
    }
}