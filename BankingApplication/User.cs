using System.Collections.Generic;

namespace BankingApplication
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Card CardDetails { get; set; }
        public int PinCode { get; set; }
        public List<Transaction> TransactionHistory { get; set; }
    }
}