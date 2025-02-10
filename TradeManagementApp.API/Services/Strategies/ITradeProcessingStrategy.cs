using TradeManagementApp.Models;

namespace TradeManagementApp.API.Services.Strategies
{
    public interface ITradeProcessingStrategy
    {
        void ProcessTrade(Trade trade);
    }
}