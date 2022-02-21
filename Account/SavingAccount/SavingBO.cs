using BankingV1._8.Account.Log;
using BankingV1._8.Menu;
using BankingV1._8.UserFolder;
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

        
        public override Account NewAccount()
        {
            bool validBalance = false;
            float balance, interest = 10;

            Account account = new Saving(UserBO.user.UserID, interest);
            account.AccountType = "Savings account";

            do
            {
                Console.WriteLine("\nType account alias");
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
            AddAccount(account, new Operation("New Account", account,0));
            return account;

        }

        public override bool AddAccount(Account a, Operation operation)
        {
            Saving s = a as Saving;
            SqlParameter[] parameters = new SqlParameter[12];
            parameters[0] = new SqlParameter("@alias", s.AccountAlias);
            parameters[1] = new SqlParameter("@type", 2);
            parameters[2] = new SqlParameter("@balance", s.Balance);
            parameters[3] = new SqlParameter("@userID", UserBO.user.UserID);
            parameters[4] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[5] = new SqlParameter("@interest", 5);
            parameters[6] = new SqlParameter("@creditLimit", DBNull.Value);
            parameters[7] = new SqlParameter("@createdAt", DateTime.Now);

            parameters[8] = new SqlParameter("@operationType", operation.OperationType);
            parameters[9] = new SqlParameter("@amount", operation.Amount);
            parameters[10] = new SqlParameter("@currentBalance", ((Account)operation.Account).Balance);
            parameters[11] = new SqlParameter("@operationDate", operation.Date);
            bool res = new AccountDataAccess().Store(parameters);
            return res;
        }

        public override void UpdateAccount(Account a, Operation operation)
        {
            Saving s = a as Saving;

            SqlParameter[] parameters = new SqlParameter[11];
            parameters[1] = new SqlParameter("@alias", s.AccountAlias);
            parameters[2] = new SqlParameter("@balance", s.Balance);
            parameters[3] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[4] = new SqlParameter("@interest", s.Interest );
            parameters[5] = new SqlParameter("@creditLimit", DBNull.Value);
            parameters[6] = new SqlParameter("@accountID", s.AccountID);
            parameters[0] = new SqlParameter("@userID", UserBO.user.UserID);

            parameters[7] = new SqlParameter("@operationType", operation.OperationType);
            parameters[8] = new SqlParameter("@amount", operation.Amount);
            parameters[9] = new SqlParameter("@currentBalance", ((Account)operation.Account).Balance);
            parameters[10] = new SqlParameter("@operationDate", operation.Date);

            bool res = new AccountDataAccess().Update(parameters);

            if (res)
                Console.WriteLine("Your change is saved");
            else
                Console.WriteLine("An error occurred while updating");

        }
    }
}
