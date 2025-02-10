using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Models;
using TradeManagementApp.Persistence.Repositories;

namespace TradeManagementApp.API.Services
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

        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            return await _tradeRepository.GetAllTradesAsync();
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _tradeRepository.GetTradeByIdAsync(id);
        }

        public async Task AddTradeAsync(Trade trade)
        {
            if (await _accountRepository.GetAccountByIdAsync(trade.AccountId) != null)
            {
                await _tradeRepository.AddTradeAsync(trade);
            }
            else
            {
                throw new KeyNotFoundException("Account ID does not exist.");
            }
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            if (await _accountRepository.GetAccountByIdAsync(trade.AccountId) != null)
            {
                await _tradeRepository.UpdateTradeAsync(trade);
            }
            else
            {
                throw new KeyNotFoundException("Account ID does not exist.");
            }
        }

        public async Task DeleteTradeAsync(int id)
        {
            var trade = await _tradeRepository.GetTradeByIdAsync(id);
            if (trade != null)
            {
                if (await _accountRepository.GetAccountByIdAsync(trade.AccountId) != null)
                {
                    await _tradeRepository.DeleteTradeAsync(id);
                }
                else
                {
                    throw new KeyNotFoundException("Account ID does not exist.");
                }
            }
        }
    }
}