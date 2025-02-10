using System.Collections.Generic;

namespace TradeManagementApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Trade> Trades { get; set; }
    }
}