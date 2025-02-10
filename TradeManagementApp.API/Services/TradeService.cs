using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Models;
using TradeManagementApp.Persistence.Repositories;
using TradeManagementApp.API.Services.Strategies;

namespace TradeManagementApp.API.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly ITradeProcessingStrategy _tradeProcessingStrategy;

        public TradeService(ITradeRepository tradeRepository, ITradeProcessingStrategy tradeProcessingStrategy)
        {
            _tradeRepository = tradeRepository;
            _tradeProcessingStrategy = tradeProcessingStrategy;
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
            _tradeProcessingStrategy.ProcessTrade(trade);
            await _tradeRepository.AddTradeAsync(trade);
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            _tradeProcessingStrategy.ProcessTrade(trade);
            await _tradeRepository.UpdateTradeAsync(trade);
        }

        public async Task DeleteTradeAsync(int id)
        {
            await _tradeRepository.DeleteTradeAsync(id);
        }
    }
}