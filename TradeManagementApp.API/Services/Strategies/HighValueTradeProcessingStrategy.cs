using TradeManagementApp.Models;

namespace TradeManagementApp.API.Services.Strategies
{
    public class HighValueTradeProcessingStrategy : ITradeProcessingStrategy
    {
        public void ProcessTrade(Trade trade)
        {
            // High-value trade processing logic
            if (trade.Amount > 100000)
            {
                trade.Status = "HighValueProcessed";
            }
            else
            {
                trade.Status = "Processed";
            }
        }
    }
}