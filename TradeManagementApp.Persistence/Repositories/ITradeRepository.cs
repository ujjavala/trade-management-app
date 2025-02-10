using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Models;

namespace TradeManagementApp.Persistence.Repositories
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