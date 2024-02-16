using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NLog;

namespace BankingApplication
{
    public class Transaction
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public double AmountGEL { get; set; }
        public double AmountUSD { get; set; }
        public double AmountEUR { get; set; }
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            
            Logger.Info("Created Transaction {transType} GEL: {gel} USD: {usd} EUR: {eur}", transactionType, amountGEL, amountUSD, amountEUR);
            
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
        
        // SOME LOGIC NEEDS TO BE FIXED HERE
        public static void CheckBalance(User currentUser)
        {
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;
            
            Logger.Info("User Checked Balane");
            CreateTransaction(currentUser, "CheckBalance");
            Console.WriteLine($@"Amount GEL: {amountGEL}
Amount USD: {amountUSD}
Amount EUR: {amountEUR}");
        }
        
        public static void Withdraw(User currentUser)
        {
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;
            Console.Write("Enter from which balance you want to withdraw money(GEL, USD, EUR): ");
            var withdrawalBalance = Console.ReadLine();
            switch (withdrawalBalance)
            {
                case "GEL":
                    Console.Write("Enter the amount you want to withdraw: ");
                    try
                    {
                        var withdrawalAmountGEL = Convert.ToDouble(Console.ReadLine());
                        if (withdrawalAmountGEL > amountGEL)
                        {
                            Console.WriteLine("Insufficient Balance");
                            CreateTransaction(currentUser, "Withdraw");

                        }
                        else
                        {
                            Console.WriteLine($"You have withdrawn {withdrawalAmountGEL} GEL");
                            var newGELBalance = amountGEL - withdrawalAmountGEL;
                            CreateTransaction(currentUser, "Withdraw", newGELBalance, amountUSD, amountEUR);
                        
                        }  
                    }
                    catch (Exception e)
                    {
                        Logger.Info($"Exception thrown: {e}");
                        Console.WriteLine("Invalid Amount!");
                    }
                    break;
                case "USD":
                    Console.Write("Enter the amount you want to withdraw: ");
                    try
                    {
                        var withdrawalAmountUSD = Convert.ToDouble(Console.ReadLine());
                        if (withdrawalAmountUSD > amountUSD)
                        {
                            Console.WriteLine("Insufficient Balance");
                            CreateTransaction(currentUser, "Withdraw");

                        }
                        else
                        {
                            Console.WriteLine($"You have withdrawn {withdrawalAmountUSD} USD");
                            var newUSDBalance = amountUSD - withdrawalAmountUSD;
                            CreateTransaction(currentUser, "Withdraw", amountGEL, newUSDBalance, amountEUR);
                        
                        }  
                    }
                    catch (Exception e)
                    {
                        Logger.Info($"Exception thrown: {e}");
                        Console.WriteLine("Invalid Amount!");
                    }
                    break;
                case "EUR":
                    Console.Write("Enter the amount you want to withdraw: ");
                    try
                    {
                        var withdrawalAmountEUR = Convert.ToDouble(Console.ReadLine());
                        if (withdrawalAmountEUR > amountEUR)
                        {
                            Console.WriteLine("Insufficient Balance");
                            CreateTransaction(currentUser, "Withdraw");

                        }
                        else
                        {
                            Console.WriteLine($"You have withdrawn {withdrawalAmountEUR} EUR");
                            var newEURBalance = amountEUR - withdrawalAmountEUR;
                            CreateTransaction(currentUser, "Withdraw", amountGEL, amountUSD, newEURBalance);
                        
                        }  
                    }
                    catch (Exception e)
                    {
                        Logger.Info($"Exception thrown: {e}");
                        Console.WriteLine("Invalid Amount!");
                    }
                    
                    
                    break;
                default:
                    Console.WriteLine("Invalid Balance");
                    break;
            }
        }
        
        public static void Last5Transactions(User currentUser)
        {
            if (currentUser.TransactionHistory.Count > 5)
            {
                
                var last5Transactions = currentUser.TransactionHistory.Skip(currentUser.TransactionHistory.Count - 5)
                    .ToList();
                CreateTransaction(currentUser, "Last5Transactions");
                foreach (var transaction in last5Transactions)
                {
                    Console.WriteLine(transaction.ToString());
                }
            }
            else
            {
                foreach (var transaction in currentUser.TransactionHistory)
                {
                    Console.WriteLine(transaction.ToString());
                }
            }
        }

    }
}