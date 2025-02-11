using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;

namespace TradeManagementApp.Domain.Repositories
{
    public interface ITradeRepository
    {
        Task<IEnumerable<Trade>> GetAllTradesAsync();
        Task<Trade> GetTradeByIdAsync(int id);
        Task AddTradeAsync(Trade trade);
        Task UpdateTradeAsync(Trade trade);
        Task DeleteTradeAsync(int id);
    }
}