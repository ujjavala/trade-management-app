using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Models;
using TradeManagementApp.Persistence.Repositories;

namespace TradeManagementApp.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAccountsAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _accountRepository.GetAccountByIdAsync(id);
        }

        public async Task AddAccountAsync(Account account)
        {
            await _accountRepository.AddAccountAsync(account);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAccountAsync(id);
        }
    }
}