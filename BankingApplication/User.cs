using System;
using System.Collections.Generic;
using NLog;

namespace BankingApplication
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Card CardDetails { get; set; }
        public int PinCode { get; set; }
        public List<Transaction> TransactionHistory { get; set; }
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public bool PinCodeCheck()
        {
            Logger.Info("PinCOdeCheck method called");
            Console.Write("Enter your Pin Code: ");
            var pinString = Console.ReadLine();
            if (int.TryParse(pinString, out var pin))
            {
                Logger.Info("Users input PIN code");
                return PinCode == pin;
            }

            Logger.Info("Users input was an incorrect PIN");
            return false;
        }
    }
}