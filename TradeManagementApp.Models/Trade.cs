using System;

namespace TradeManagementApp.Models
{
    public class Trade
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string SecurityCode { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
        public string BuyOrSell { get; set; }
        public string Status { get; set; }
        public Account Account { get; set; }
    }
}