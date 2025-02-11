using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeManagementApp.Application.Services;
using TradeManagementApp.Domain.Models;

namespace TradeManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return Ok(await _accountService.GetAllAccountsAsync());
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _accountService.GetAccountByIdWithCacheAsync(id); // Use the cached method

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // GET: api/Accounts/Search?id=123&lastName=Doe
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Account>>> SearchAccounts(int? id, string? lastName)
        {
            if (id == null && string.IsNullOrEmpty(lastName))
            {
                return BadRequest("At least one search parameter (id or lastName) must be provided.");
            }

            // Validate lastName to prevent SQL injection
            if (!string.IsNullOrEmpty(lastName) && lastName.Length > 50) // Example max length
            {
                return BadRequest("Last name is too long.");
            }

            var accounts = await _accountService.SearchAccountsAsync(id, lastName);

            // Return an empty list if no accounts are found
            if (accounts == null || !accounts.Any())
            {
                return Ok(new List<Account>());
            }

            return Ok(accounts);
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Account>> PutAccount(int id, Account account)
        {
            // Use the Id from the URL if no Id is supplied in the body
            if (account.Id == 0)
            {
                account.Id = id;
            }

            if (id != account.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the body of the request.");
            }

            try
            {
                await _accountService.UpdateAccountAsync(account);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            // Return the updated account
            var updatedAccount = await _accountService.GetAccountByIdAsync(id);
            return Ok(updatedAccount);
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            try
            {
                await _accountService.AddAccountAsync(account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}