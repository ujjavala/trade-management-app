using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Models;

namespace TradeManagementApp.API.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task AddAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
    }
}