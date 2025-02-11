using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Domain.Repositories;

namespace TradeManagementApp.Persistence.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly DataContext _context;

        public TradeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            return await _context.Trades.ToListAsync();
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task AddTradeAsync(Trade trade)
        {
            await _context.Trades.AddAsync(trade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            var existingTrade = await _context.Trades.FindAsync(trade.Id);
            if (existingTrade != null)
            {
                _context.Entry(existingTrade).CurrentValues.SetValues(trade);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new DbUpdateConcurrencyException("Attempted to update or delete an entity that does not exist in the store.");
            }
        }

        public async Task DeleteTradeAsync(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade != null)
            {
                _context.Trades.Remove(trade);
                await _context.SaveChangesAsync();
            }
        }
    }
}