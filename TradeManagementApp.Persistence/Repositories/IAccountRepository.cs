using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Models;

namespace TradeManagementApp.Persistence.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task AddAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
    }
}