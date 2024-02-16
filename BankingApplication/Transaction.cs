using System;
using System.Linq;
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

        public static void CreateTransaction(User currentUser, string transactionType, double amountGEL,
            double amountUSD, double amountEUR)
        {
            Logger.Info("Created Transaction {transType} GEL: {gel} USD: {usd} EUR: {eur}", transactionType, amountGEL,
                amountUSD, amountEUR);

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
            Logger.Info("Created Transaction {transType}", transactionType);
            currentUser.TransactionHistory.Add(new Transaction()
            {
                TransactionDate = DateTime.Now,
                TransactionType = transactionType,
                AmountGEL = currentUser.TransactionHistory.Last().AmountGEL,
                AmountUSD = currentUser.TransactionHistory.Last().AmountUSD,
                AmountEUR = currentUser.TransactionHistory.Last().AmountEUR
            });
        }

        public static void CheckBalance(User currentUser)
        {
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;

            Logger.Info("User Checked Balance");
            CreateTransaction(currentUser, "CheckBalance");
            Console.WriteLine($@"Amount GEL: {amountGEL}
Amount USD: {amountUSD}
Amount EUR: {amountEUR}");
        }

        public static void Withdraw(User currentUser)
        {
            Logger.Info("Withdraw method called");
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
                            Logger.Info("User tried to withdraw, but had insuficcient balance");
                            Console.WriteLine("Insufficient Balance");
                            CreateTransaction(currentUser, "Withdraw");
                        }
                        else
                        {
                            Logger.Info("User withdrew {withdrawnAmountGEL} GEL", withdrawalAmountGEL);
                            Console.WriteLine($"You have withdrawn {withdrawalAmountGEL} GEL");
                            var newGELBalance = amountGEL - withdrawalAmountGEL;
                            CreateTransaction(currentUser, "Withdraw", newGELBalance, amountUSD, amountEUR);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Info(e, "Exception caught");
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
                            Logger.Info("User tried to withdraw, but had insuficcient balance");
                            Console.WriteLine("Insufficient Balance");
                            CreateTransaction(currentUser, "Withdraw");
                        }
                        else
                        {
                            Logger.Info("User withdrew {withdrawnAmountUSD} USD", withdrawalAmountUSD);
                            Console.WriteLine($"You have withdrawn {withdrawalAmountUSD} USD");
                            var newUSDBalance = amountUSD - withdrawalAmountUSD;
                            CreateTransaction(currentUser, "Withdraw", amountGEL, newUSDBalance, amountEUR);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Info(e, "Exception caught");
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
                            Logger.Info("User tried to withdraw, but had insuficcient balance");
                            Console.WriteLine("Insufficient Balance");
                            CreateTransaction(currentUser, "Withdraw");
                        }
                        else
                        {
                            Logger.Info("User withdrew {withdrawnAmountEUR} EUR", withdrawalAmountEUR);
                            Console.WriteLine($"You have withdrawn {withdrawalAmountEUR} EUR");
                            var newEURBalance = amountEUR - withdrawalAmountEUR;
                            CreateTransaction(currentUser, "Withdraw", amountGEL, amountUSD, newEURBalance);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Info(e, "Exception caught");
                        Console.WriteLine("Invalid Amount!");
                    }


                    break;
                default:
                    Logger.Info("User tried to withdraw but chose Invalid Balance");
                    Console.WriteLine("Invalid Balance");
                    break;
            }
        }

        public static void Last5Transactions(User currentUser)
        {
            Logger.Info("User Checked their Last 5 Transactions");
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

        public static void FillBalance(User currentUser)
        {
            Logger.Info("FillBalance method called");
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;
            Console.Write("Enter which balance you would like to fill: ");
            var balanceToFill = Console.ReadLine();
            switch (balanceToFill)
            {
                case "GEL":
                    Console.Write("Enter amount to fill by: ");
                    try
                    {
                        var amountToFillByGEL = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine($"GEL balance filled by {amountToFillByGEL}");
                        CreateTransaction(currentUser, "FillBalance", amountGEL + amountToFillByGEL, amountUSD,
                            amountEUR);
                        Logger.Info("User filled their GEL balance by {filledBy}", amountToFillByGEL);
                    }
                    catch (Exception e)
                    {
                        Logger.Info(e, "Exception Caught");
                        Console.WriteLine("Invalid Input");
                    }

                    break;
                case "USD":
                    Console.Write("Enter amount to fill by: ");
                    try
                    {
                        var amountToFillByUSD = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine($"USD balance filled by {amountToFillByUSD}");
                        CreateTransaction(currentUser, "FillBalance", amountGEL, amountUSD + amountToFillByUSD,
                            amountEUR);
                        Logger.Info("User filled their USD balance by {filledBy}", amountToFillByUSD);
                    }
                    catch (Exception e)
                    {
                        Logger.Info(e, "Exception Caught");
                        Console.WriteLine("Invalid Input");
                    }

                    break;
                case "EUR":
                    Console.Write("Enter amount to fill by: ");
                    try
                    {
                        var amountToFillByEUR = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine($"EUR balance filled by {amountToFillByEUR}");
                        CreateTransaction(currentUser, "FillBalance", amountGEL, amountUSD,
                            amountEUR + amountToFillByEUR);
                        Logger.Info("User filled their EUR balance by {filledBy}", amountToFillByEUR);
                    }
                    catch (Exception e)
                    {
                        Logger.Info(e, "Exception Caught");
                        Console.WriteLine("Invalid Input");
                    }

                    break;
                default:
                    Logger.Info("Fill balance method called, but user put invalid Balance name");
                    Console.WriteLine("Invalid Balance");
                    break;
            }
        }

        public static void ChangePIN(User currentUser)
        {
            Logger.Info("ChangePIN method called");
            Console.Write("Enter old PIN Code: ");
            try
            {
                var oldPIN = Convert.ToInt32(Console.ReadLine());
                if (currentUser.PinCode == oldPIN)
                {
                    Console.Write("Enter new PIN Code: ");
                    var newPinString = Console.ReadLine();
                    if (int.TryParse(newPinString, out var newPIN) & (newPIN >= 1000 & newPIN <= 9999))
                    {
                        currentUser.PinCode = newPIN;
                        Console.WriteLine($"PIN changed to {newPIN}");
                        CreateTransaction(currentUser, "ChangePIN");
                        Logger.Info("User's PIN code changed to {changedPIN}", newPIN);
                    }
                    else
                    {
                        Console.WriteLine("Invalid PIN Code");
                        Logger.Info("User input an Invalid PIN code");
                    }
                }
                else
                {
                    Console.WriteLine("The PIN you entered doesn't match your old PIN code");
                    Logger.Info("User input an incorrect old PIN");
                }
            }
            catch (Exception e)
            {
                Logger.Info(e, "Exception caught");
                Console.WriteLine("Invalid PIN code");
            }
        }

        public static void CurrencyConversion(User currentUser)
        {
            Logger.Info("CurrencyConversion method called");
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;
            Console.Write("Enter from which currency you want to convert: ");
            var toConvertFrom = Console.ReadLine();
            Console.Write("Convert to: ");
            var convertTo = Console.ReadLine();

            switch (toConvertFrom)
            {
                case "GEL":
                    switch (convertTo)
                    {
                        case "USD":
                            Console.Write("Enter amount to convert: ");
                            try
                            {
                                var amountToConvertGELtoUSD = Convert.ToDouble(Console.ReadLine());
                                if (amountToConvertGELtoUSD <= amountGEL)
                                {
                                    Logger.Info("Converted from GEL to USD");
                                    CreateTransaction(currentUser, "CurrencyConversion",
                                        amountGEL - amountToConvertGELtoUSD,
                                        amountUSD + (0.38 * amountToConvertGELtoUSD), amountEUR);
                                }
                                else
                                {
                                    Logger.Info("User tried to convert but entered invalid amount");
                                    Console.WriteLine("Invalid amount to convert");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Info(e, "Exception Caught");
                                Console.WriteLine("Invalid amount");
                            }

                            break;
                        case "EUR":
                            Console.Write("Enter amount to convert: ");
                            try
                            {
                                var amountToConvertGELtoEUR = Convert.ToDouble(Console.ReadLine());
                                if (amountToConvertGELtoEUR <= amountGEL)
                                {
                                    Logger.Info("Converted from GEL to EUR");
                                    CreateTransaction(currentUser, "CurrencyConversion",
                                        amountGEL - amountToConvertGELtoEUR, amountUSD,
                                        amountEUR + (0.35 * amountToConvertGELtoEUR));
                                }
                                else
                                {
                                    Logger.Info("User tried to convert but entered invalid amount");
                                    Console.WriteLine("Invalid amount to convert");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Info(e, "Exception Caught");
                                Console.WriteLine("Invalid amount");
                            }

                            break;
                        default:
                            Logger.Info("User tried to convert but entered an Invalid Currency");
                            Console.WriteLine("Invalid Currency");
                            break;
                    }

                    break;
                case "USD":
                    switch (convertTo)
                    {
                        case "GEL":
                            Console.Write("Enter amount to convert: ");
                            try
                            {
                                var amountToConvertUSDtoGEL = Convert.ToDouble(Console.ReadLine());
                                if (amountToConvertUSDtoGEL <= amountGEL)
                                {
                                    Logger.Info("Converted from USD to GEL");
                                    CreateTransaction(currentUser, "CurrencyConversion",
                                        amountGEL + (2.64 * amountToConvertUSDtoGEL),
                                        amountUSD - amountToConvertUSDtoGEL, amountEUR);
                                }
                                else
                                {
                                    Logger.Info("User tried to convert but entered invalid amount");
                                    Console.WriteLine("Invalid amount to convert");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Info(e, "Exception Caught");
                                Console.WriteLine("Invalid amount");
                            }

                            break;
                        case "EUR":
                            Console.Write("Enter amount to convert: ");
                            try
                            {
                                var amountToConvertUSDtoEUR = Convert.ToDouble(Console.ReadLine());
                                if (amountToConvertUSDtoEUR <= amountGEL)
                                {
                                    Logger.Info("Converted from USD to EUR");
                                    CreateTransaction(currentUser, "CurrencyConversion", amountGEL,
                                        amountUSD - amountToConvertUSDtoEUR,
                                        amountEUR + (0.93 * amountToConvertUSDtoEUR));
                                }
                                else
                                {
                                    Logger.Info("User tried to convert but entered invalid amount");
                                    Console.WriteLine("Invalid amount to convert");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Info(e, "Exception Caught");
                                Console.WriteLine("Invalid amount");
                            }

                            break;
                        default:
                            Logger.Info("User tried to convert but entered an Invalid Currency");
                            Console.WriteLine("Invalid Currency");
                            break;
                    }

                    break;
                case "EUR":
                    switch (convertTo)
                    {
                        case "GEL":
                            Console.Write("Enter amount to convert: ");
                            try
                            {
                                var amountToConvertEURtoGEL = Convert.ToDouble(Console.ReadLine());
                                if (amountToConvertEURtoGEL <= amountGEL)
                                {
                                    Logger.Info("Converted from EUR to GEL");
                                    CreateTransaction(currentUser, "CurrencyConversion",
                                        amountGEL + (2.85 * amountToConvertEURtoGEL), amountUSD,
                                        amountEUR - amountToConvertEURtoGEL);
                                }
                                else
                                {
                                    Logger.Info("User tried to convert but entered invalid amount");
                                    Console.WriteLine("Invalid amount to convert");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Info(e, "Exception Caught");
                                Console.WriteLine("Invalid amount");
                            }

                            break;
                        case "USD":
                            Console.Write("Enter amount to convert: ");
                            try
                            {
                                var amountToConvertEURtoUSD = Convert.ToDouble(Console.ReadLine());
                                if (amountToConvertEURtoUSD <= amountGEL)
                                {
                                    Logger.Info("Converted from EUR to USD");
                                    CreateTransaction(currentUser, "CurrencyConversion", amountGEL,
                                        amountUSD + (1.08 * amountToConvertEURtoUSD),
                                        amountEUR - amountToConvertEURtoUSD);
                                }
                                else
                                {
                                    Logger.Info("User tried to convert but entered invalid amount");
                                    Console.WriteLine("Invalid amount to convert");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Info(e, "Exception Caught");
                                Console.WriteLine("Invalid amount");
                            }

                            break;
                        default:
                            Logger.Info("User tried to convert but entered an Invalid Currency");
                            Console.WriteLine("Invalid Currency");
                            break;
                    }

                    break;
                default:
                    Logger.Info("User tried to convert but entered an Invalid Currency");
                    Console.WriteLine("Invalid Currency");
                    break;
            }
        }
    }
}