using TradeManagementApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeManagementApp.Application.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task AddAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
        Task<Account> GetAccountByIdWithCacheAsync(int id); // New method
    }
}