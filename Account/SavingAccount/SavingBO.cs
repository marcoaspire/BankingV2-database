using BankingV1._8.Account.Receipt;
using BankingV1._8.Menu;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account.SavingAccount
{
    class SavingBO : AccountBO
    {
        public override float MonthEndBalance(Account account)
        {
            //interest earning per month
            Saving savAccount = (Saving)account;
            Console.WriteLine(savAccount.Balance + savAccount.Balance * savAccount.Interest / (100 * 12));
            return savAccount.Balance + savAccount.Balance * savAccount.Interest / (100 * 12);
        }

        public override Account NewAccount()
        {
            bool validAccount, validBalance = false;
            long accountNumber;
            float balance, interest = 10;

            Account account = new Saving(BankMenu.userID, interest);
            account.AccountType = "Savings account";


            do
            {
                Console.WriteLine("\nType your account numbers");
                validAccount = Int64.TryParse(Console.ReadLine(), out accountNumber);
                account.AccountNumber = accountNumber;

                //check if it's not in list



            } while (!validAccount);

            do
            {
                Console.WriteLine("Type account name");
                account.AccountAlias = Console.ReadLine();
                if (string.IsNullOrEmpty(account.AccountAlias))
                    Console.WriteLine("Error:Name can not be empty, try again");
            } while (string.IsNullOrEmpty(account.AccountAlias));

            do
            {
                Console.WriteLine("Type the balance");
                validBalance = float.TryParse(Console.ReadLine(), out balance);
                //validate balance
            } while (!validBalance || !CheckBalance(balance));
            account.Balance = balance;


            //operation
            AddAccount(account, new Operation("New Account", account, 0, 0));
            //Console.WriteLine($"Hello dear user, you have a new saving account {account.AccountAlias} , its account number is {account.AccountNumber} and you have ${account.Balance}");
            return account;

        }

        public override bool AddAccount(Account a, Operation operation)
        {
            Saving s = a as Saving;
            SqlParameter[] parameters = new SqlParameter[14];
            parameters[0] = new SqlParameter("@number", s.AccountNumber);
            parameters[1] = new SqlParameter("@alias", s.AccountAlias);
            parameters[2] = new SqlParameter("@type", 2);
            parameters[3] = new SqlParameter("@balance", s.Balance);
            parameters[4] = new SqlParameter("@userID", BankMenu.userID);
            parameters[5] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[6] = new SqlParameter("@interest", 5);
            parameters[7] = new SqlParameter("@creditLimit", DBNull.Value);
            parameters[8] = new SqlParameter("@createdAt", DateTime.Now);

            parameters[9] = new SqlParameter("@operationType", operation.OperationType);
            parameters[10] = new SqlParameter("@amount", operation.Amount);
            parameters[11] = new SqlParameter("@currentBalance", ((Account)operation.Account.Clone()).Balance);
            parameters[12] = new SqlParameter("@previousBalance", operation.PreviousBalance);
            parameters[13] = new SqlParameter("@operationDate", operation.Date);
            bool res = new AccountDataAccess().Store(parameters);
            return res;
        }

        public override void UpdateAccount(Account a)
        {
            Saving s = a as Saving;

            SqlParameter[] parameters = new SqlParameter[8];
            parameters[0] = new SqlParameter("@number", s.AccountNumber);
            parameters[1] = new SqlParameter("@alias", s.AccountAlias);
            parameters[2] = new SqlParameter("@balance", s.Balance);
            parameters[3] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[4] = new SqlParameter("@interest", s.Interest );
            parameters[5] = new SqlParameter("@creditLimit", DBNull.Value);
            parameters[6] = new SqlParameter("@accountID", s.AccountID);
            parameters[7] = new SqlParameter("@userID", BankMenu.userID);


            bool res = new AccountDataAccess().Update(parameters);

            if (res)
                Console.WriteLine("Your change is saved");
            else
                Console.WriteLine("error al cambiar saving");

        }
    }
}
