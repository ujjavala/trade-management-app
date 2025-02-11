using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeManagementApp.Application.Services;
using TradeManagementApp.Domain.Models;

namespace TradeManagementApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly ITradeService tradeService;

        public TradesController(ITradeService tradeService)
        {
            this.tradeService = tradeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trade>>> GetAllTrades()
        {
            var trades = await tradeService.GetAllTradesAsync();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trade>> GetTradeById(int id)
        {
            var trade = await tradeService.GetTradeByIdAsync(id);
            if (trade == null)
            {
                return NotFound();
            }

            return Ok(trade);
        }

        [HttpPost]
        public async Task<ActionResult<Trade>> CreateTrade(Trade trade)
        {
            try
            {
                await tradeService.AddTradeAsync(trade);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetTradeById), new { id = trade.Id }, trade);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Trade>> UpdateTrade(int id, Trade trade)
        {
            // Use the Id from the URL if no Id is supplied in the body
            if (trade.Id == 0)
            {
                trade.Id = id;
            }

            if (id != trade.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the body of the request.");
            }

            try
            {
                await tradeService.UpdateTradeAsync(trade);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            // Return the updated trade
            var updatedTrade = await tradeService.GetTradeByIdAsync(id);
            return Ok(updatedTrade);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            try
            {
                await tradeService.DeleteTradeAsync(id);
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