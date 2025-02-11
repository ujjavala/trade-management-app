using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Domain.Repositories;

namespace TradeManagementApp.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Account>> SearchAccountsAsync(int? id, string? lastName)
        {
            IQueryable<Account> query = _context.Accounts;

            if (id.HasValue)
            {
                query = query.Where(a => a.Id == id.Value);
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                // Use EF.Functions.Like with parameterization
                query = query.Where(a => EF.Functions.Like(a.LastName.ToLower(), $"%{lastName.ToLower()}%"));
            }

            return await query.ToListAsync();
        }
    }
}