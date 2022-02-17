using BankingV1._8.Account.CreditAccount;
using BankingV1._8.Account.CurrentAccount;
using BankingV1._8.Account.Receipt;
using BankingV1._8.Account.SavingAccount;
using BankingV1._8.Menu;
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
        public void UpdateAlias(Account a)
        {
            String alias;
            do
            {
                Console.WriteLine("\nWrite the new name of the account2");
                alias = Console.ReadLine();
                if (string.IsNullOrEmpty(alias))
                    Console.WriteLine("Error:Name can not be empty");
            } while (string.IsNullOrEmpty(alias));
            a.AccountAlias = alias;
            this.UpdateAccount(a);
        }
        public virtual void Deposit(Account account)
        {
            bool validDeposit = false;
            float deposit;
            do
            {
                Console.WriteLine("Type the amount you want to deposit");
                validDeposit = float.TryParse(Console.ReadLine(), out deposit);
            } while (!validDeposit || deposit < 0);
            float previo = account.Balance;
            account.Balance += deposit;
            //OperationBO.operations.Add(DateTime.Now, new Operation("Deposit", (Account)account.Value.Clone(), accountAuxiliary.Balance, deposit));
            this.UpdateAccount(account);
            Console.WriteLine($"Now your balance is ${account.Balance}");
            //return account;
        }
        public virtual void Withdraw(Account account)
        {
            float withdrawal;
            bool validWithdrawal = false;

            do
            {
                Console.WriteLine("Type the amount you want to withdraw");
                validWithdrawal = float.TryParse(Console.ReadLine(), out withdrawal);
            } while (!validWithdrawal);
            if (withdrawal > account.Balance)
            {
                Console.WriteLine($"Your balance is less than {withdrawal} \n Transaction failed");
            }
            else
            {
                //Account auxiliar = (Account)account.Value.Clone();
                account.Balance -= withdrawal;
                //OperationBO.operations.Add(DateTime.Now, new Operation("Withdraw", (Account)account.Value.Clone(), auxiliar.Balance, withdrawal));
                this.UpdateAccount(account);
                
                Console.WriteLine($"Now your balance is ${account.Balance}");

            }
            //return account;
        }

        public virtual float MonthEndBalance(Account account)
        {
            return account.Balance;
        }
        /*
        public static void ShowAllAcounts()
        {
            Console.WriteLine("Your accounts are:");
            foreach (Account item in AccountBO.accounts)
            {
                if (BankMenu.email_session.Equals(item.Owner))
                    Console.WriteLine(item.ToString());
            }

        }
        */
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
        public static Account FindAccount(long accountNumber)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@userID", BankMenu.userID);
            parameters[1] = new SqlParameter("@accountNumber", accountNumber);
            DataSet res = new AccountDataAccess().SearchAccountByNumber(parameters);
            DataRow acc;
            Account account =null;
            if (res.Tables[0].Rows.Count > 0)
            {
                acc = res.Tables[0].Rows[0];
                Console.WriteLine(acc["AccountType"]);
                switch (Convert.ToInt32(acc["AccountType"]))
                {
                    case 1:
                        account = new Current(Convert.ToInt32(acc["AccountID"].ToString()),BankMenu.userID, acc["AccountAlias"].ToString(), Convert.ToInt64(acc["AccountNumber"].ToString()),
                            "Current account", float.Parse(acc["Balance"].ToString()), float.Parse(acc["DepositLimit"].ToString()));
                        break;
                    case 2:
                        account = new Saving(Convert.ToInt32(acc["AccountID"].ToString()),BankMenu.userID, acc["AccountAlias"].ToString(), Convert.ToInt64(acc["AccountNumber"].ToString()),
                            "Saving account", float.Parse(acc["Balance"].ToString()), float.Parse(acc["Interest"].ToString()));
                        break;
                    case 3:
                        account = new Credit(Convert.ToInt32(acc["AccountID"].ToString()),BankMenu.userID, acc["AccountAlias"].ToString(), Convert.ToInt64(acc["AccountNumber"].ToString()),
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
                Console.WriteLine("Balance must be positive. Try again");
                return false;
            }
            return true;
        }
        //interface methods
        public abstract Account NewAccount();

        public virtual void RemoveAccount(Account a)
        {
            if (a.Balance != 0)
                Console.WriteLine("You need to withdraw all your money before to delete");
            else
            {
                //OperationBO.operations.Add(DateTime.Now, new Operation("Account deleted", a, a.Balance, 0));
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@userID", BankMenu.userID);
                parameters[1] = new SqlParameter("@accountID", a.AccountID);


                bool res = new AccountDataAccess().Delete(parameters);

                if (res)
                    Console.WriteLine("Account deleted");
                else
                    Console.WriteLine("Error, we could not delete your account");

            }
        }

        public abstract void UpdateAccount(Account a);

        public abstract bool AddAccount(Account u, Operation operation);

        public static void FindAccountsByUser()
        {
            SqlParameter parameter = new SqlParameter();
            parameter = new SqlParameter("@userID", BankMenu.userID);

            //List<Account>accounts = new List<Account>();
            DataSet res = new AccountDataAccess().SearchAccountByUser(parameter);
            
            if (res.Tables[0].Rows.Count>0)
                foreach (DataRow account in res.Tables[0].Rows)
                {
                    //TODO: CHANGE 
                    Console.WriteLine("{0} {1} {2} {3}",account["AccountNumber"], account["AccountAlias"], 
                        account["Balance"], account["AccountType"]);
                }
            else
            {
                Console.WriteLine("You don't have an account with us. Open your account now!");
            }
        }

    }
}
