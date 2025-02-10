using TradeManagementApp.Models;

namespace TradeManagementApp.API.Services.Strategies
{
    public class StandardTradeProcessingStrategy : ITradeProcessingStrategy
    {
        public void ProcessTrade(Trade trade)
        {
            // Standard trade processing logic
            trade.Status = "Processed";
        }
    }
}