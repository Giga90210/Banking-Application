using System;

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
    }
}