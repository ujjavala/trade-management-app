using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TradeManagementApp.Models
{
    public class Trade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account ID is required")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Security Code is required")]
        [StringLength(3, ErrorMessage = "Security Code must be exactly 3 characters")]
        public string SecurityCode { get; set; }

        [Required(ErrorMessage = "Timestamp is required")]
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Buy or Sell is required")]
        public string BuyOrSell { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public TradeStatus Status { get; set; }

        // Navigation property
        [JsonIgnore]
        public Account Account { get; set; }
    }
}