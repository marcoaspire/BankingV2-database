using BankingV1._8.Account;
using BankingV1._8.Account.CreditAccount;
using BankingV1._8.Account.CurrentAccount;
using BankingV1._8.Account.Receipt;
using BankingV1._8.Account.SavingAccount;
using BankingV1._8.UserFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Menu
{
    class BankMenu
    {
        //it´s like a session variable
        public static int userID; //change to User class
        //Session["UserID"];


        public bool Register()
        {
            string password = "", email,firstName,lastName;
            bool validEmail = false;
            UserBO usBO = new UserBO();
            do
            {
                Console.WriteLine("\n\nSign Up");
                Console.WriteLine("Type your name");
                firstName = Console.ReadLine();
                Console.WriteLine("Type your lastName");
                lastName = Console.ReadLine();
                Console.WriteLine("Type your email");
                email = Console.ReadLine();
                try
                {
                    email = new MailAddress(email).Address;
                    validEmail = true;
                    Console.WriteLine("\nType your password, it needs to follow the below rules");
                    Console.WriteLine("It should contain at least one uppercase and lowercase alphabets");
                    Console.WriteLine("It should contain at least one numerical value");
                    Console.WriteLine("It should contain at least one special character");
                    Console.WriteLine("It should not contain any whitespaces");
                    Console.WriteLine("The length of the password should more than 7 characters");
                    password = Console.ReadLine();
                    if (!UserBO.ValidatePassword(password))
                    {
                        throw new Exception("The password does not meet the requirements, try again, please");
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid format");
                }
                catch (FormatException)
                {

                    Console.WriteLine("Please provide a valid email address");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            } while (!UserBO.ValidatePassword(password) || !validEmail);
            User newUser = new User(email, password, firstName, lastName);
            if (usBO.AddUser(newUser))
                Console.WriteLine("User saved");
            else
            {
                Console.WriteLine("We could not save");
            }
            try
            {
                /*
                if (UserBO.CheckEmail(email))
                    throw new Exception("Someone already has this email address. Try again, please.\n");
                else
                {
                    //UserBO.users.AddLast(newUser);
                    //save in DB
                    bool res = new UserBO().AddUser(newUser);
                    if (res)
                        Console.WriteLine("User saved");
                    else
                        Console.WriteLine("Error");
                    //FileBO.SaveUsers();
                }
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        public void LogIn()
        {

            bool verification = false;
            string password,email;
            bool validEmail = false;
            do
            {
                Console.WriteLine("\nLogin");
                Console.WriteLine("Email");
                email = Console.ReadLine();
                try
                {
                    email = new MailAddress(email).Address;
                    Console.WriteLine("\nPassword");
                    password = Console.ReadLine();
                    //search in DB
                    UserBO usBO = new UserBO();
                    User u = new User(email, password);
                    User userValidated = usBO.Find(u);
                    if (userValidated != null)
                    {
                        Console.WriteLine(userValidated.UserID);
                        verification = true;
                        DisplayMenu();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect email address or password. Please try again.");

                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Email format is invalid");
                   
                }

                
                //try
                //{
                    
                //}
                //catch (ArgumentException)
                //{
                //    Console.WriteLine("Invalid format");
                //}
                //catch (FormatException)
                //{
                //    Console.WriteLine("Please provide a valid email address");
                //}
                //catch (Exception e)
                //{

                //    Console.WriteLine(e.Message);
                //}

            } while (!verification);

        }
        public void LoginMenu()
        {
            bool validNumber;
            int choice;
            do
            {
                Console.WriteLine("Welcome Banking System");
                Console.WriteLine("1-Login");
                Console.WriteLine("2-Register");
                Console.WriteLine("3-Exit");
                validNumber = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice);
                if (choice > 3 || choice < 1)
                    Console.WriteLine("\nInvalid option. Please enter a number between 1 and 3.");
            } while (!validNumber || choice > 3 || choice < 1);
            if (choice == 1)
            {
                LogIn();
            }
            else if (choice == 2)
            {
                if (Register())
                    LogIn();
                else
                    LoginMenu();
            }
            else
            {
                Console.WriteLine("\nThank you for using this system");
            }
        }
        public void DisplayMenu()
        {
            int op, choice;
            bool validNumber;
            CurrentBO currentBO = new CurrentBO();
            CreditBO creditBO = new CreditBO();
            SavingBO savingBO = new SavingBO();
            OperationBO operationBO = new OperationBO();
            do
            {
                Console.WriteLine("Welcome Banking System");
                Console.WriteLine("1.Open a new bank account");
                Console.WriteLine("2.Display your account details by its number");
                Console.WriteLine("3.Display all your accounts");
                Console.WriteLine("4.Deposit/Pay your credit");
                Console.WriteLine("5.Withdraw/Pay with your credit");
                //Console.WriteLine("6.End of the month, get your new balance");
                Console.WriteLine("7.Delete/Change name of your account");
                Console.WriteLine("8.Exit");
                Console.WriteLine("Type an option, please");
                op = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (op)
                {
                    case '1':
                        do
                        {
                            Console.WriteLine("Which type of bank account would you like to open?");
                            Console.WriteLine("1-Current account");
                            Console.WriteLine("2-Savings account");
                            Console.WriteLine("3-Credit account");
                            Console.WriteLine("4-Return to menu");
                            validNumber = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice);
                            if (choice > 4 || choice < 1)
                                Console.WriteLine("Invalid option. Please enter a number between 1 and 4.");

                        } while (!validNumber || choice > 4 || choice < 1);
                        if (choice == 4)
                            break;
                        
                        Account.Account newAccount = null;

                        switch (choice)
                        {
                            case 1:
                                newAccount = currentBO.NewAccount();
                                /*
                                if (currentBO.AddAccount(newAccount))
                                {
                                    Console.WriteLine("You hava a new account");
                                    //operation TODO: HERE
                                    //operationBO.AddOperation(new Operation("New Account", newAccount, 0, 0));
                                }
                                else
                                    Console.WriteLine("error");
                                */
                                break;
                            case 2:
                                newAccount = savingBO.NewAccount();
                                /*
                                if (savingBO.AddAccount(newAccount))
                                {

                                    Console.WriteLine("You hava a new saving account");
                                }
                                else
                                    Console.WriteLine("error");
                                */
                                break;
                            case 3:
                                newAccount = creditBO.NewAccount();
                                /*
                                if (creditBO.AddAccount(newAccount))
                                {

                                    Console.WriteLine("You hava a new credit account");
                                }
                                else
                                    Console.WriteLine("error");
                                */
                                break;
                            default:
                                break;
                        }
                        try
                        {
                            //TODO: Check if tha account has not been registered
                            /*
                            if (!AccountBO.accounts.Contains(newAccount))
                            {
                                AccountBO.accounts.AddLast(newAccount);
                                OperationBO.operations.Add(DateTime.Now, new Operation("NewAccount", (Account.Account)newAccount.Clone(), newAccount.Balance, 0));
                            }
                            else
                                Console.WriteLine("Error: We could not open the new account because Someone already has that account number.");
                            */
                        }
                        catch (InvalidCastException)
                        {
                            Console.WriteLine("Specified cast is not valid");
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Error");
                        }

                        break;
                    case '2':
                        Account.Account accountFound = AccountBO.AskAccountNumber();
                        if (accountFound != null)
                        {
                            Console.WriteLine(accountFound.ToString());
                        }
                        break;
                    case '3':
                            AccountBO.FindAccountsByUser();
                        break;
                    case '4':
                        accountFound = AccountBO.AskAccountNumber();
                        if (accountFound != null)
                        {
                            string type = accountFound.AccountType;
                            if (type.Equals("Credit account"))
                            {
                                creditBO.Deposit(accountFound);
                            }
                            else if (type.Equals("Current account"))
                            {
                                currentBO.Deposit(accountFound);
                            }
                            else if (type.Equals("Saving account"))
                            {
                                savingBO.Deposit(accountFound);
                            }
                        }
                        /*
                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.AskAccountNumber();
                            if (accoutFound != null)
                            {
                                try
                                {
                                    string type = accoutFound.Value.GetType().Name;

                                    if (type.Equals("Credit"))
                                    {
                                        creditBO.Deposit(accoutFound);
                                    }
                                    else if (type.Equals("Current"))
                                    {
                                        currentBO.Deposit(accoutFound);
                                    }
                                    else if (type.Equals("Saving"))
                                    {
                                        savingBO.Deposit(accoutFound);
                                    }
                                }
                                catch (InvalidCastException)
                                {
                                    Console.WriteLine("Specified cast is not valid");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Transaction has failed. " + e.Message);
                                }



                            }
                            else
                                Console.WriteLine("We couldn’t find account with that number ");


                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        */
                        break;
                        
                    case '5':
                        accountFound = AccountBO.AskAccountNumber();
                        if (accountFound != null)
                        {
                            string type = accountFound.AccountType;
                            if (type.Equals("Credit account"))
                            {
                                creditBO.Withdraw(accountFound);
                            }
                            else if (type.Equals("Current account"))
                            {
                                currentBO.Withdraw(accountFound);
                            }
                            else if (type.Equals("Saving account"))
                            {
                                savingBO.Withdraw(accountFound);
                            }
                        }
                        /*
                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.AskAccountNumber();


                            if (accoutFound != null)
                            {
                                string type = accoutFound.Value.GetType().Name;

                                if (type.Equals("Credit"))
                                {
                                    creditBO.Withdraw(accoutFound);
                                }
                                else if (type.Equals("Current"))
                                {
                                    currentBO.Withdraw(accoutFound);
                                }
                                else if (type.Equals("Saving"))
                                {
                                    savingBO.Withdraw(accoutFound);
                                }
                            }
                            else
                            {
                                Console.WriteLine("We couldn’t find account with that number ");
                            }


                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        */
                        break;
                    case '6':
                        /*
                        if (AccountBO.accounts != null)
                        {
                            LinkedListNode<Account.Account> accoutFound = AccountBO.AskAccountNumber();
                            try
                            {
                                if (accoutFound != null)
                                {
                                    string type = accoutFound.Value.GetType().Name;
                                    Console.WriteLine("Account Statement");

                                    if (type.Equals("Credit"))
                                    {
                                        accoutFound.Value.Balance = creditBO.MonthEndBalance((Credit)accoutFound.Value);
                                        Console.WriteLine();
                                        Console.WriteLine(accoutFound.Value.ToString());
                                    }
                                    else if (type.Equals("Current"))
                                    {
                                        accoutFound.Value.Balance = currentBO.MonthEndBalance(accoutFound.Value);

                                        Console.WriteLine(accoutFound.Value.ToString());
                                    }
                                    else if (type.Equals("Saving"))
                                    {
                                        accoutFound.Value.Balance = savingBO.MonthEndBalance(accoutFound.Value);
                                        Console.WriteLine(accoutFound.Value.ToString());
                                    }
                                }
                                else
                                    Console.WriteLine("We couldn’t find account with that number");
                            }
                            catch (InvalidCastException)
                            {
                                Console.WriteLine("Specified cast is not valid");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error:" + e.Message);
                            }

                        }
                        else
                            Console.WriteLine("You don't have an account with us. Open your account now!");
                        */
                        break;
                    case '7':
                        Account.Account accoutFound = AccountBO.AskAccountNumber();
                        if (accoutFound != null)
                        {
                            do
                            {
                                Console.WriteLine("1- Modify the name of your account");
                                Console.WriteLine("2- Delete your account");
                                Console.WriteLine("3- Cancel");
                                validNumber = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice);
                                if (choice > 3 || choice < 1)
                                    Console.WriteLine("Invalid option. Please enter a number between 1 and 3.");

                            } while (!validNumber || choice > 3 || choice < 1);
                            if (choice == 3)
                                break;

                            try
                            {
                                string type = accoutFound.AccountType;
                                if (type.Equals("Credit account"))
                                {
                                    Credit account = (Credit)accoutFound;
                                    if (choice == 1)
                                    {
                                        creditBO.UpdateAlias(account);
                                    }
                                    else
                                        creditBO.DeleteAccount(account);
                                }
                                else if (type.Equals("Current account"))
                                {
                                    Current account = (Current)accoutFound;
                                    if (choice == 1)
                                    {
                                        currentBO.UpdateAlias(account);
                                    }
                                    else
                                        currentBO.DeleteAccount(account);
                                }
                                else if (type.Equals("Saving account"))
                                {
                                    Saving account = (Saving)accoutFound;
                                    if (choice == 1)
                                        savingBO.UpdateAlias(account);
                                    else
                                        savingBO.DeleteAccount(account);
                                }
                            }
                            catch (InvalidCastException)
                            {
                                Console.WriteLine("Specified cast is not valid");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error:" + e.Message);
                            }

                        }
                        else
                            Console.WriteLine("We couldn’t find account with that number");
                        
                        break;
                    case '8':
                        Console.WriteLine("Thank you for using this system");
                        userID = -1;
                        //new OperationBO().PrintOperations();
                        //FileBO.SaveData();
                        LoginMenu();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 1 and 8.");
                        break;

                }
                Console.WriteLine("");

            } while (op != '8');



            Console.ReadKey();
        }







    }
}
