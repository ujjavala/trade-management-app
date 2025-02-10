// <copyright file="ITradeService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TradeManagementApp.API.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TradeManagementApp.Models;

    public interface ITradeService
    {
        Task<IEnumerable<Trade>> GetAllTradesAsync();

        Task<Trade> GetTradeByIdAsync(int id);

        Task AddTradeAsync(Trade trade);

        Task UpdateTradeAsync(Trade trade);

        Task DeleteTradeAsync(int id);
    }
}
