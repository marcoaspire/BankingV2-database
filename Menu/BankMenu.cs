using BankingV1._8.Account;
using BankingV1._8.Account.CreditAccount;
using BankingV1._8.Account.CurrentAccount;
using BankingV1._8.Account.Log;
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
        

        public bool Register()
        {
            string password = "", email,firstName,lastName;
            bool valid = false;
            UserBO usBO = new UserBO();
            do
            {
                try
                {
                
                    Console.WriteLine("\n\nSign Up");
                    Console.WriteLine("Type your name");
                    firstName = Console.ReadLine();
                    if (!UserBO.ValidateName(firstName))
                        throw new Exception("Name must has at least two characters ( just letters), try again, please");
                    Console.WriteLine("Type your last name");
                    lastName = Console.ReadLine();
                    if (!UserBO.ValidateName(lastName))
                        throw new Exception("Last name must has at least two characters ( just letters), try again, please");
                    Console.WriteLine("Type your email");
                    email = Console.ReadLine();
                
                    email = new MailAddress(email).Address;
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
                
                    User newUser = new User(email, password, firstName, lastName);
                    if (usBO.AddUser(newUser))
                    {
                        Console.WriteLine("User saved");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("We could not register\n");

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
            } while (!valid);
            return false;
        }
        public void LogIn()
        {

            bool verification = false;
            string password,email;
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
                    UserBO usBO = new UserBO();
                    User u = new User(email, password);
                    User userValidated = usBO.Find(u);
                    if (userValidated != null)
                    {
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
                Console.WriteLine("4.Exit");
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
                                Console.WriteLine("\nInvalid option. Please enter a number between 1 and 4.");

                        } while (!validNumber || choice > 4 || choice < 1);
                        if (choice == 4)
                            break;
                        
                        Account.Account newAccount = null;

                        switch (choice)
                        {
                            case 1:
                                newAccount = currentBO.NewAccount();
                                break;
                            case 2:
                                newAccount = savingBO.NewAccount();
                                break;
                            case 3:
                                newAccount = creditBO.NewAccount();
                                break;
                            default:
                                break;
                        }
                        
                        break;
                    case '2':
                        Account.Account accountFound = AccountBO.AskAccountNumber();
                        if (accountFound != null)
                        {
                            Console.WriteLine(accountFound.ToString());
                            do
                            {
                                Console.WriteLine("1.Deposit/Pay your credit");
                                Console.WriteLine("2.Withdraw/Pay with your credit");
                                Console.WriteLine("3-Change name of your account");
                                Console.WriteLine("4.Delete your account");
                                Console.WriteLine("5-Return to menu");
                                validNumber = Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice);
                                if (choice > 5 || choice < 1)
                                    Console.WriteLine("\nInvalid option. Please enter a number between 1 and 5.");

                            } while (!validNumber || choice > 5 || choice < 1);
                            switch (choice)
                            {
                                case 1:
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
                                    
                                    break;

                                case 2:
                                    
                                    type = accountFound.AccountType;
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
                                    
                                    break;
                                case 3:
                                    type = accountFound.AccountType;
                                    if (type.Equals("Credit account"))
                                    {
                                        Credit account = (Credit)accountFound;
                                        creditBO.UpdateAlias(account);
                                    }
                                    else if (type.Equals("Current account"))
                                    {
                                        Current account = (Current)accountFound;
                                        currentBO.UpdateAlias(account);
                                    }
                                    else if (type.Equals("Saving account"))
                                    {
                                        Saving account = (Saving)accountFound;
                                        savingBO.UpdateAlias(account);
                                    }
                                    else
                                    {
                                        Console.WriteLine("tipo mal");
                                    }
                                    break;
                                case 4:
                                    type = accountFound.AccountType;
                                    if (type.Equals("Credit account"))
                                    {
                                        Credit account = (Credit)accountFound;
                                        creditBO.DeleteAccount(account);
                                    }
                                    else if (type.Equals("Current account"))
                                    {
                                        Current account = (Current)accountFound;
                                        currentBO.DeleteAccount(account);
                                    }
                                    else if (type.Equals("Saving account"))
                                    {
                                        Saving account = (Saving)accountFound;
                                        savingBO.DeleteAccount(account);
                                    }

                                break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            Console.WriteLine("We couldn’t find account with that number");
                        }
                        break;
                    case '3':
                            AccountBO.FindAccountsByUser();
                        break;
                    case '4':
                        Console.WriteLine("Your session ended, thank you.");
                        UserBO.user = null;
                        LoginMenu();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 1 and 7.");
                        break;

                }
                Console.WriteLine("");

            } while (op != '4');



            Console.ReadKey();
        }







    }
}
