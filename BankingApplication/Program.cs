using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace BankingApplication
{
    class Program
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logger.Info("Application Started!");
            run();
            LogManager.Shutdown();
        }

        static void run()
        {
            var test123Text =
                File.ReadAllText(
                    "C:\\Users\\Home\\RiderProjects\\BankingApplication\\BankingApplication\\users.json");
            var userList = JsonConvert.DeserializeObject<List<User>>(test123Text);
            while (true)
            {
                var currentUser = new User()
                {
                    PinCode = -1
                };
                Console.Write("Enter your card number:");
                var inputCardNumber = Console.ReadLine();
                Console.Write("Enter the expiration date of your card: ");
                var inputExpirationDate = Console.ReadLine();
                Console.Write("Enter your card verification code(CVC): ");
                int inputCVC;
                try
                {
                    inputCVC = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Logger.Info(e, "Exception caught");
                    Console.WriteLine("Please enter valid CVC");
                    break;
                }

                foreach (var user in userList)
                {
                    if (user.CardDetails.CardNumber == inputCardNumber &&
                        user.CardDetails.ExpirationDate == inputExpirationDate && user.CardDetails.CVC == inputCVC)
                    {
                        currentUser = user;
                        Logger.Info("User Found!");
                    }
                }

                if (currentUser.PinCode == -1)
                {
                    Console.WriteLine("\nNo such user exists in our database, please try again\n");
                    Logger.Info("User Not Found! restarting application");
                    break;
                }

                if (!currentUser.PinCodeCheck())
                {
                    Console.WriteLine("\nPlease Provide Correct PIN!\n");
                    Logger.Info("Incorrect PIN! restarting application");
                    break;
                }

                Console.WriteLine(
                    "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                Console.WriteLine($@"
                            Hello {currentUser.FirstName} {currentUser.LastName}

                            1) View Balance
                            2) Withdraw
                            3) Last 5 Transactions
                            4) Fill Balance
                            5) Change PIN
                            6) Currency Conversion");
                var transactionTypeString = Console.ReadLine();
                if (int.TryParse(transactionTypeString, out var transactionType))
                {
                    switch (transactionType)
                    {
                        case 1:
                            Transaction.CheckBalance(currentUser);
                            break;
                        case 2:
                            Transaction.Withdraw(currentUser);
                            break;
                        case 3:
                            Transaction.Last5Transactions(currentUser);
                            break;
                        case 4:
                            Transaction.FillBalance(currentUser);
                            break;
                        case 5:
                            Transaction.ChangePIN(currentUser);
                            break;
                        case 6:
                            Transaction.CurrencyConversion(currentUser);
                            break;
                        default:
                            Console.WriteLine("Invalid Operation");
                            Logger.Info("Invalid Operation! restarting application");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                    Logger.Info("Invalid input! restarting application");
                }

                var _dataConverted = JsonConvert.SerializeObject(userList);
                File.WriteAllText(@"C:\Users\Home\RiderProjects\BankingApplication\BankingApplication\users.json",
                    _dataConverted);
            }
        }
    }
}