using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.API.Services;
using TradeManagementApp.Models;

namespace TradeManagementApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradesController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trade>>> GetAllTrades()
        {
            var trades = await _tradeService.GetAllTradesAsync();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trade>> GetTradeById(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);
            if (trade == null)
            {
                return NotFound();
            }
            return Ok(trade);
        }

        [HttpPost]
        public async Task<ActionResult<Trade>> CreateTrade(Trade trade)
        {
            await _tradeService.AddTradeAsync(trade);
            return CreatedAtAction(nameof(GetTradeById), new { id = trade.Id }, trade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrade(int id, Trade trade)
        {
            if (id != trade.Id)
            {
                return BadRequest();
            }

            await _tradeService.UpdateTradeAsync(trade);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            await _tradeService.DeleteTradeAsync(id);
            return NoContent();
        }
    }
}