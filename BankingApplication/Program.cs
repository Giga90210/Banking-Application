using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace BankingApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var test123Text = File.ReadAllText("C:\\Users\\Home\\RiderProjects\\BankingApplication\\BankingApplication\\test123.json");
            var userList = JsonConvert.DeserializeObject<List<User>>(test123Text);
            while (true)
            {
                var currentUser = new User()
                {
                    FirstName = "XXXX"
                };
                Console.Write("Enter your card number:");
                var inputCardNumber = Console.ReadLine();
                Console.Write("Enter the expiration date of your card: ");
                var inputExpirationDate = Console.ReadLine();
                Console.Write("Enter your card verification code(CVC): ");
                var inputCVC = Convert.ToInt32(Console.ReadLine());
                
                foreach (var user in userList)
                {
                    if (user.CardDetails.CardNumber == inputCardNumber & user.CardDetails.ExpirationDate == inputExpirationDate & user.CardDetails.CVC == inputCVC)
                    {
                        currentUser = user;
                    }
                }

                if (currentUser.FirstName == "XXXX")
                {
                    Console.WriteLine("\nNo such user exists in our database, please try again\n");
                    break;
                }
                if (!PinCodeCheck(currentUser))
                {
                    Console.WriteLine("\nPlease Provide Correct PIN!\n");
                    break;
                }

                Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
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
                            CheckBalance(currentUser);
                            break;
                        case 2:
                            Withdraw(currentUser);
                            break;
                        case 3:
                            Last5Transactions(currentUser);
                            break;
                        case 4:
                            FillBalance(currentUser);
                            break;
                        case 5:
                            ChangePIN(currentUser);
                            break;
                        case 6:
                            CurrencyConversion(currentUser);
                            break;
                        default:
                            Console.WriteLine("Invalid Operation");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
                var _dataConverted = JsonConvert.SerializeObject(userList);
                File.WriteAllText(@"C:\Users\Home\RiderProjects\BankingApplication\BankingApplication\test123.json", _dataConverted);
            }
        }

        public static bool PinCodeCheck(User currentUser)
        {
            Console.Write("Enter your Pin Code: ");
            var pinString = Console.ReadLine();
            if (int.TryParse(pinString, out var pin))
            {
                return currentUser.PinCode == pin;
            }

            return false;
        }

        // public static void CreateTransaction(User currentUser, string transactionType, double amountGEL, double amountUSD, double amountEUR)
        // {
        //     currentUser.TransactionHistory.Add(new Transaction()
        //     {
        //         TransactionDate = DateTime.Now,
        //         TransactionType = transactionType,
        //         AmountGEL = amountGEL,
        //         AmountUSD = amountUSD,
        //         AmountEUR = amountEUR
        //     });
        // }
        //
        // public static void CreateTransaction(User currentUser, string transactionType)
        // {
        //     currentUser.TransactionHistory.Add(new Transaction()
        //     {
        //         TransactionDate = DateTime.Now,
        //         TransactionType = transactionType,
        //         AmountGEL = currentUser.TransactionHistory.Last().AmountGEL,
        //         AmountUSD = currentUser.TransactionHistory.Last().AmountUSD,
        //         AmountEUR = currentUser.TransactionHistory.Last().AmountEUR
        //     });
        // }
        public static void CheckBalance(User currentUser)
        {
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;
            if (currentUser.TransactionHistory.Count != 0)
            {
                Transaction.CreateTransaction(currentUser, "CheckBalance");
                Console.WriteLine($@"Amount GEL: {amountGEL}
Amount USD: {amountUSD}
Amount EUR: {amountEUR}");

            }
            else
            {
                Transaction.CreateTransaction(currentUser, "CheckBalance", 0, 0, 0);
                // LOGIC MISSING HERE

                Console.WriteLine("You haven't filled your balance!");
            }
 

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
                    var withdrawalAmountGELString = Console.ReadLine();
                    if (double.TryParse(withdrawalAmountGELString, out var withdrawalAmountGEL))
                    {
                        if (withdrawalAmountGEL > amountGEL)
                        {
                            Console.WriteLine("Insufficient Balance");
                            Transaction.CreateTransaction(currentUser, "Withdraw");

                        }
                        else
                        {
                            Console.WriteLine($"You have withdrawn {withdrawalAmountGEL} Gel");
                            var newGELBalance = amountGEL - withdrawalAmountGEL;
                            Transaction.CreateTransaction(currentUser, "Withdraw", newGELBalance, amountUSD, amountEUR);
                        
                        }   
                    }
                    else
                    {
                        Console.WriteLine("Invalid Amount");
                    }
                    break;
                case "USD":
                    Console.Write("Enter the amount you want to withdraw: ");
                    var withdrawalAmountUSDString = Console.ReadLine();
                    if (double.TryParse(withdrawalAmountUSDString, out var withdrawalAmountUSD))
                    {
                        if (withdrawalAmountUSD > amountUSD)
                        {
                            Console.WriteLine("Insufficient Balance");
                            Transaction.CreateTransaction(currentUser, "Withdraw");

                        }
                        else
                        {
                            Console.WriteLine($"You have withdrawn {withdrawalAmountUSD} USD");
                            var newUSDBalance = amountUSD - withdrawalAmountUSD;
                            Transaction.CreateTransaction(currentUser, "Withdraw", amountGEL, newUSDBalance, amountEUR);
                        
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Amount");
                    }
                    break;
                case "EUR":
                    Console.Write("Enter the amount you want to withdraw: ");
                    var withdrawalAmountEURString = Console.ReadLine();
                    if (double.TryParse(withdrawalAmountEURString, out var withdrawalAmountEUR))
                    {
                        if (withdrawalAmountEUR > amountEUR)
                        {
                            Console.WriteLine("Insufficient Balance");
                            Transaction.CreateTransaction(currentUser, "Withdraw");

                        }
                        else
                        {
                            Console.WriteLine($"You have withdrawn {withdrawalAmountEUR} EUR");
                            var newEURBalance = amountEUR - withdrawalAmountEUR;
                            Transaction.CreateTransaction(currentUser, "Withdraw", amountGEL, amountUSD, newEURBalance);
                        
                        } 
                    }
                    else
                    {
                        Console.WriteLine("Invalid Amount");
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
                Transaction.CreateTransaction(currentUser, "Last5Transactions");
                var last5Transactions = currentUser.TransactionHistory.Skip(currentUser.TransactionHistory.Count - 5)
                    .ToList();
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
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;
            Console.Write("Enter which balance you would like to fill: ");
            var balanceToFill = Console.ReadLine();
            switch (balanceToFill)
            { 
                case "GEL":
                    Console.Write("Enter amount to fill by: ");
                    var amountToFillStringGEL = Console.ReadLine();
                    if (double.TryParse(amountToFillStringGEL, out var amountToFillByGEL))
                    {
                        Console.WriteLine($"GEL balance filled by {amountToFillByGEL}");
                        Transaction.CreateTransaction(currentUser, "FillBalance", amountGEL + amountToFillByGEL, amountUSD, amountEUR);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }
                    break;
                case "USD":
                    Console.Write("Enter amount to fill by: ");
                    var amountToFillStringUSD = Console.ReadLine();
                    if (double.TryParse(amountToFillStringUSD, out var amountToFillByUSD))
                    {
                        Console.WriteLine($"GEL balance filled by {amountToFillByUSD}");
                        Transaction.CreateTransaction(currentUser, "FillBalance", amountGEL, amountUSD + amountToFillByUSD, amountEUR);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }
                    break;
                case "EUR":
                    Console.Write("Enter amount to fill by: ");
                    var amountToFillStringEUR = Console.ReadLine();
                    if (double.TryParse(amountToFillStringEUR, out var amountToFillByEUR))
                    {
                        Console.WriteLine($"GEL balance filled by {amountToFillByEUR}");
                        Transaction.CreateTransaction(currentUser, "FillBalance", amountGEL, amountUSD, amountEUR + amountToFillByEUR);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid Balance");
                    break;
            }
        }

        public static void ChangePIN(User currentUser)
        {
            Console.Write("Enter old PIN Code: ");
            var oldPinString = Console.ReadLine();
            if (int.TryParse(oldPinString, out var oldPIN))
            {
                if (currentUser.PinCode == oldPIN)
                {
                    Console.Write("Enter new PIN Code: ");
                    var newPinString = Console.ReadLine();
                    if (int.TryParse(newPinString, out var newPIN) & (newPIN >= 1000 & newPIN <= 9999))
                    {
                        currentUser.PinCode = newPIN;
                        Console.WriteLine($"PIN changed to {newPIN}");
                        Transaction.CreateTransaction(currentUser, "ChangePIN");
                    }
                    else
                    {
                        Console.WriteLine("Invalid PIN Code");
                    }
                }
                else
                {
                    Console.WriteLine("The PIN Code that you entered doesn't match your old one");
                }
            }
            else
            {
                Console.WriteLine("Invalid PIN Code");
            }
        }

        public static void CurrencyConversion(User currentUser)
        {
            var amountGEL = currentUser.TransactionHistory.Last().AmountGEL;
            var amountUSD = currentUser.TransactionHistory.Last().AmountUSD;
            var amountEUR = currentUser.TransactionHistory.Last().AmountEUR;
            Console.Write("Enter from which currency you want to convert: ");
            var toConvertFrom = Console.ReadLine();
            Console.Write("Convert to: ");
            var convertTo = Console.ReadLine();

            switch (toConvertFrom)
            {
                case "GEl":
                    switch (convertTo)
                    {
                        case "USD":
                            Console.Write("Enter amount to convert: ");
                            var amountToConvertStringGELtoUSD = Console.ReadLine();
                            if (double.TryParse(amountToConvertStringGELtoUSD, out var amountToConvertGELtoUSD))
                            {
                                if (amountToConvertGELtoUSD <= amountGEL)
                                {
                                    Transaction.CreateTransaction(currentUser, "CurrencyConversion", amountGEL - amountToConvertGELtoUSD, amountUSD + (0.38*amountToConvertGELtoUSD), amountEUR);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount");
                            }
                            break;
                        case "EUR":
                            Console.Write("Enter amount to convert: ");
                            var amountToConvertStringGELtoEUR = Console.ReadLine();
                            if (double.TryParse(amountToConvertStringGELtoEUR, out var amountToConvertGELtoEUR))
                            {
                                if (amountToConvertGELtoEUR <= amountGEL)
                                {
                                    Transaction.CreateTransaction(currentUser, "CurrencyConversion", amountGEL - amountToConvertGELtoEUR, amountUSD , amountEUR + (0.35*amountToConvertGELtoEUR));
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid Currency");
                            break;
                    }
                    break;
                case "USD":
                    switch (convertTo)
                    {
                        case "GEL":
                            Console.Write("Enter amount to convert: ");
                            var amountToConvertStringUSDtoGEL = Console.ReadLine();
                            if (double.TryParse(amountToConvertStringUSDtoGEL, out var amountToConvertUSDtoGEL))
                            {
                                if (amountToConvertUSDtoGEL <= amountUSD)
                                {
                                    Transaction.CreateTransaction(currentUser, "CurrencyConversion", amountGEL + (2.64 * amountToConvertUSDtoGEL), amountUSD - amountToConvertUSDtoGEL, amountEUR);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount");
                            }
                            break;
                        case "EUR":
                            Console.Write("Enter amount to convert: ");
                            var amountToConvertStringUSDtoEUR = Console.ReadLine();
                            if (double.TryParse(amountToConvertStringUSDtoEUR, out var amountToConvertUSDtoEUR))
                            {
                                if (amountToConvertUSDtoEUR <= amountUSD)
                                {
                                    Transaction.CreateTransaction(currentUser, "CurrencyConversion", amountGEL, amountUSD - amountToConvertUSDtoEUR, amountEUR + (0.93 * amountToConvertUSDtoEUR));
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid Currency");
                            break;
                    }
                    break;
                case "EUR":
                    switch (convertTo)
                    {
                        case "GEL":
                            Console.Write("Enter amount to convert: ");
                            var amountToConvertStringEURtoGEL = Console.ReadLine();
                            if (double.TryParse(amountToConvertStringEURtoGEL, out var amountToConvertEURtoGEL))
                            {
                                if (amountToConvertEURtoGEL <= amountEUR)
                                {
                                    Transaction.CreateTransaction(currentUser, "CurrencyConversion", amountGEL + (2.85 * amountToConvertEURtoGEL), amountUSD, amountEUR - amountToConvertEURtoGEL);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount");
                            }
                            break;
                        case "USD":
                            Console.Write("Enter amount to convert: ");
                            var amountToConvertStringEURtoUSD = Console.ReadLine();
                            if (double.TryParse(amountToConvertStringEURtoUSD, out var amountToConvertEURtoUSD))
                            {
                                if (amountToConvertEURtoUSD <= amountEUR)
                                {
                                    Transaction.CreateTransaction(currentUser, "CurrencyConversion", amountGEL, amountUSD + (1.08 * amountToConvertEURtoUSD), amountEUR - amountToConvertEURtoUSD);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid Currency");
                            break;
                    }
                    break;                
                default:
                    Console.WriteLine("Invalid Currency");
                    break;
            }
        }
    }
}









            // var testCardJohn = new Card()
            // {
            //     CardNumber = "1111-2222-3333-4444",
            //     ExpirationDate = "05/26",
            //     CVC = 111
            // };
            // var testUserJohn = new User()
            // {
            //     FirstName = "John",
            //     LastName = "Doe",
            //     CardDetails = testCardJohn,
            //     PinCode = 1111,
            //     TransactionHistory = new List<Transaction>()
            // };
            //
            //
            //
            //
            // var testCardJane = new Card()
            // {
            //     CardNumber = "1234-5678-4321-8765",
            //     ExpirationDate = "11/25",
            //     CVC = 123
            // };
            // var testUserJane = new User()
            // {
            //     FirstName = "Jane",
            //     LastName = "Doe",
            //     CardDetails = testCardJane,
            //     PinCode = 1234,
            //     TransactionHistory = new List<Transaction>()
            // };
            //
            //
            //
            //
            // var testCardMichael = new Card()
            // {
            //     CardNumber = "9999-1234-5678-0000",
            //     ExpirationDate = "03/28",
            //     CVC = 236
            // };
            // var testUserMichael = new User()
            // {
            //     FirstName = "Michael",
            //     LastName = "Jordan",
            //     CardDetails = testCardMichael,
            //     PinCode = 2003,
            //     TransactionHistory = new List<Transaction>()
            // };
            //
            //
            //
            //
            // var testCardKobe = new Card()
            // {
            //     CardNumber = "1234-5678-4321-8765",
            //     ExpirationDate = "12/26",
            //     CVC = 245
            // };
            // var testUserKobe = new User()
            // {
            //     FirstName = "Kobe",
            //     LastName = "Bryant",
            //     CardDetails = testCardKobe,
            //     PinCode = 2016,
            //     TransactionHistory = new List<Transaction>()
            // };
            //
            //
            //
            // var testUserList = new List<User>();
            // testUserList.Add(testUserJohn);
            // testUserList.Add(testUserJane);
            // testUserList.Add(testUserMichael);
            // testUserList.Add(testUserKobe);
            // var _dataConverted = JsonConvert.SerializeObject(testUserList);
            // File.WriteAllText(@"C:\Users\Home\RiderProjects\BankingApplication\BankingApplication\test123.json", _dataConverted);




// var testTrans = new Transaction()
// {
//     TransactionDate = DateTime.Now,
//     TransactionType = TransactionType.ViewBalance,
//     AmountGEL = 100,
//     AmountUSD = 0,
//     AmountEUR = 0
// };
// var testTrans1 = new Transaction()
// {
//     TransactionDate = DateTime.Now,
//     TransactionType = TransactionType.ChangePIN,
//     AmountGEL = 100,
//     AmountUSD = 0,
//     AmountEUR = 0
// };
// var testTrans2 = new Transaction()
// {
//     TransactionDate = DateTime.Now,
//     TransactionType = TransactionType.CurrencyConversion,
//     AmountGEL = 100,
//     AmountUSD = 0,
//     AmountEUR = 0
// };
// var testTrans3 = new Transaction()
// {
//     TransactionDate = DateTime.Now,
//     TransactionType = TransactionType.Withdraw,
//     AmountGEL = 100,
//     AmountUSD = 0,
//     AmountEUR = 0
// };
// var testTrans4 = new Transaction()
// {
//     TransactionDate = DateTime.Now,
//     TransactionType = TransactionType.FillBalance,
//     AmountGEL = 100,
//     AmountUSD = 0,
//     AmountEUR = 0
// };
// testUser.TransactionHistory.Add(testTrans);
// testUser.TransactionHistory.Add(testTrans1);
// testUser.TransactionHistory.Add(testTrans2);
// testUser.TransactionHistory.Add(testTrans3);
// testUser.TransactionHistory.Add(testTrans4);