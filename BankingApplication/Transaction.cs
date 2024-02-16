using System;
using System.Linq;

namespace BankingApplication
{
    public class Transaction
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public double AmountGEL { get; set; }
        public double AmountUSD { get; set; }
        public double AmountEUR { get; set; }

        public override string ToString()
        {
            return @$"Transaction Date: {TransactionDate}
Transaction Type: {TransactionType}
Amount GEL: {AmountGEL}
Amount USD: {AmountUSD}
Amount EUR: {AmountEUR}";
        }
        
        public static void CreateTransaction(User currentUser, string transactionType, double amountGEL, double amountUSD, double amountEUR)
        {
            currentUser.TransactionHistory.Add(new Transaction()
            {
                TransactionDate = DateTime.Now,
                TransactionType = transactionType,
                AmountGEL = amountGEL,
                AmountUSD = amountUSD,
                AmountEUR = amountEUR
            });
        }
        
        public static void CreateTransaction(User currentUser, string transactionType)
        {
            currentUser.TransactionHistory.Add(new Transaction()
            {
                TransactionDate = DateTime.Now,
                TransactionType = transactionType,
                AmountGEL = currentUser.TransactionHistory.Last().AmountGEL,
                AmountUSD = currentUser.TransactionHistory.Last().AmountUSD,
                AmountEUR = currentUser.TransactionHistory.Last().AmountEUR
            });
        }
    }
}