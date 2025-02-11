// <copyright file="ITradeService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using TradeManagementApp.Domain.Models;

namespace TradeManagementApp.Application.Services
{
    public interface ITradeService
    {
        Task<IEnumerable<Trade>> GetAllTradesAsync();

        Task<Trade> GetTradeByIdAsync(int id);

        Task AddTradeAsync(Trade trade);

        Task UpdateTradeAsync(Trade trade);

        Task DeleteTradeAsync(int id);
    }
}
