// <copyright file="TradesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using TradeManagementApp.API.Services;
    using TradeManagementApp.Models;

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
            await tradeService.AddTradeAsync(trade);
            return CreatedAtAction(nameof(GetTradeById), new { id = trade.Id }, trade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrade(int id, Trade trade)
        {
            if (id != trade.Id)
            {
                return BadRequest();
            }

            await tradeService.UpdateTradeAsync(trade);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            await tradeService.DeleteTradeAsync(id);
            return NoContent();
        }
    }
}
