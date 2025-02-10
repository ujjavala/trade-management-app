using System;
using System.ComponentModel.DataAnnotations;

namespace TradeManagementApp.Models
{
    public class Trade
    {
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string SecurityCode { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public string BuyOrSell { get; set; }

        [Required]
        public TradeStatus Status { get; set; }
    }
}