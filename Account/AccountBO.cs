using BankingV1._8.Account.CreditAccount;
using BankingV1._8.Account.CurrentAccount;
using BankingV1._8.Account.Log;
using BankingV1._8.Account.SavingAccount;
using BankingV1._8.Menu;
using BankingV1._8.UserFolder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account
{
    abstract class AccountBO : IAccountBO
    {
        //Methods
        public void UpdateAlias(Account account)
        {
            String alias;
            do
            {
                Console.WriteLine("\nWrite the new name of the account2");
                alias = Console.ReadLine();
                if (string.IsNullOrEmpty(alias))
                    Console.WriteLine("Error:Name can not be empty");
            } while (string.IsNullOrEmpty(alias));
            account.AccountAlias = alias;

            this.UpdateAccount(account, new Operation("Modify alias", account, 0));
        }
        public virtual void Deposit(Account account)
        {
            bool validDeposit = false;
            float deposit;
            do
            {
                Console.WriteLine("\nType the amount you want to deposit");
                validDeposit = float.TryParse(Console.ReadLine(), out deposit);
                if (!validDeposit)
                    Console.WriteLine("Error, try again");
            } while (!validDeposit || deposit < 0);
            float previous = account.Balance;

            Operation operation = new Operation("Deposit", account, deposit);
            account.Balance += deposit;
            this.UpdateAccount(account, operation);
            Console.WriteLine($"Now your balance is ${account.Balance}");
        }
        public virtual void Withdraw(Account account)
        {
            float withdrawal;
            bool validWithdrawal = false;

            do
            {
                Console.WriteLine("\nType the amount you want to withdraw");
                validWithdrawal = float.TryParse(Console.ReadLine(), out withdrawal);
                if (!validWithdrawal)
                    Console.WriteLine("Error, try again");
            } while (!validWithdrawal || withdrawal <= 0);
            if (withdrawal > account.Balance)
            {
                Console.WriteLine($"Your balance is less than {withdrawal} \n Transaction failed");
            }
            else
            {
                float previous = account.Balance;
                Operation operation = new Operation("Withdrawal", account,withdrawal);
                account.Balance -= withdrawal;
                this.UpdateAccount(account, operation);
                Console.WriteLine($"Now your balance is ${account.Balance}");

            }
        }

        
        public static Account AskAccountNumber()
        {
            bool validAccount;
            long accountNumber;
            do
            {
                Console.WriteLine("Type the account numbrer");
                validAccount = Int64.TryParse(Console.ReadLine(), out accountNumber);
            } while (!validAccount);

            return FindAccount(accountNumber);
        }
        public static Account FindAccount(long accountID)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@userID", UserBO.user.UserID);
            parameters[1] = new SqlParameter("@accountID", accountID);
            DataSet res = new AccountDataAccess().SearchAccountByNumber(parameters);
            DataRow acc;
            Account account =null;
            if (res.Tables[0].Rows.Count > 0)
            {
                acc = res.Tables[0].Rows[0];
                switch (Convert.ToInt32(acc["AccountType"]))
                {
                    case 1:
                        account = new Current(Convert.ToInt32(acc["AccountID"].ToString()), UserBO.user.UserID, acc["AccountAlias"].ToString(), 
                            "Current account", float.Parse(acc["Balance"].ToString()), float.Parse(acc["DepositLimit"].ToString()));
                        break;
                    case 2:
                        account = new Saving(Convert.ToInt32(acc["AccountID"].ToString()), UserBO.user.UserID, acc["AccountAlias"].ToString(),
                            "Saving account", float.Parse(acc["Balance"].ToString()), float.Parse(acc["Interest"].ToString()));
                        break;
                    case 3:
                        account = new Credit(Convert.ToInt32(acc["AccountID"].ToString()), UserBO.user.UserID, acc["AccountAlias"].ToString(),
                            "Credit account", float.Parse(acc["Balance"].ToString()) , float.Parse(acc["CreditLimit"].ToString()), float.Parse(acc["Interest"].ToString()));
                        break;
                    default:
                        Console.WriteLine("error,");

                        break;
                }
                return account;
            }
            else
            {
                Console.WriteLine("We couldn't find an account with that number.");
            }
            return null;
        }
        public bool CheckBalance(float balance)
        {
            if (balance < 1)
            {
                Console.WriteLine("Balance must be a positive number. Try again");
                return false;
            }
            else return true;
        }
        public static void FindAccountsByUser()
        {
            Account a = null;
            try
            {
                SqlParameter parameter = new SqlParameter();
                parameter = new SqlParameter("@userID", UserBO.user.UserID);
                DataSet res = new AccountDataAccess().SearchAccountByUser(parameter);

                if (res.Tables[0].Rows.Count > 0)
                    foreach (DataRow account in res.Tables[0].Rows)
                    {
                        switch (Int32.Parse(account["AccountType"].ToString()))
                        {
                            case 1:

                                a = new Current(Int32.Parse(account["AccountID"].ToString()), account["AccountAlias"].ToString(), "Current Account", float.Parse(account["Balance"].ToString()), float.Parse(account["DepositLimit"].ToString()));
                                break;
                            case 2:
                                a = new Saving(Int32.Parse(account["AccountID"].ToString()), account["AccountAlias"].ToString(), "Saving Account", float.Parse(account["Balance"].ToString()), float.Parse(account["Interest"].ToString()));
                                break;
                            case 3:
                                a = new Credit(Int32.Parse(account["AccountID"].ToString()), account["AccountAlias"].ToString(), "Credit Account", float.Parse(account["Balance"].ToString()), float.Parse(account["CreditLimit"].ToString()), float.Parse(account["Interest"].ToString()));

                                break;
                            default:
                                break;
                        }
                        Console.WriteLine(a.ToString());
                    }
                else
                {
                    Console.WriteLine("You don't have an account with us. Open your account now!");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
           
        }
        //interface methods
        public abstract Account NewAccount();

        public virtual void DeleteAccount(Account a)
        {
            if (a.Balance != 0)
                Console.WriteLine("\nYou need to withdraw all your money before to delete");
            else
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@userID", UserBO.user.UserID);
                parameters[1] = new SqlParameter("@accountID", a.AccountID);


                bool res = new AccountDataAccess().Destroy(parameters);

                if (res)
                    Console.WriteLine("\nAccount deleted");
                else
                    Console.WriteLine("Error, we could not delete your account");

            }
        }

        public abstract void UpdateAccount(Account a, Operation operation);

        public abstract bool AddAccount(Account u, Operation operation);

        

    }
}
