using BankingV1._8.Account.Log;
using BankingV1._8.Menu;
using BankingV1._8.UserFolder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account.CreditAccount
{
    class CreditBO : AccountBO
    {
        
        public override void Withdraw(Account account)
        {
            float withdrawal;
            bool validWithdrawal = false;
            try
            {
                Console.WriteLine("Pay with your credit");
                Credit creditAccount = (Credit)account;
                float availableCredit = creditAccount.Limit - creditAccount.Balance;
                do
                {
                    Console.WriteLine("Type the amount you want to pay");
                    validWithdrawal = float.TryParse(Console.ReadLine(), out withdrawal);
                } while (!validWithdrawal);
                if (withdrawal > availableCredit)
                {
                    Console.WriteLine($"Your available credit is less than {withdrawal} \n Transaction failed");
                }
                else
                {
                    Console.WriteLine("credito anterior: " + availableCredit);
                    Operation operation = new Operation("Pay with credit", account, withdrawal);
                    account.Balance += withdrawal;
                    UpdateAccount(account, operation);
                    Console.WriteLine(((Credit)account).ToString());
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

        public override void Deposit(Account account)
        {
            bool validDeposit = false;
            float deposit;
            try
            {
                Credit creditAccount = (Credit)account;
                Console.WriteLine("\nPay your credit");
                do
                {
                    Console.WriteLine("Type the amount you want to pay");
                    validDeposit = float.TryParse(Console.ReadLine(), out deposit);
                } while (!validDeposit || deposit < 0);
                Operation operation = new Operation("Pay your credit", account, deposit);
                account.Balance -= deposit;

                UpdateAccount(account, operation);
                Console.WriteLine(account.ToString());
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

        public override Account NewAccount()
        {
            bool validBalance = false;
            float balance, interest = 30;

            Credit account = null;
            do
            {
                Console.WriteLine("\nHow much do you want to deposit? Your deposit will be equal to your credit limit");
                validBalance = Single.TryParse(Console.ReadLine(), out balance);

            } while (!validBalance || !CheckBalance(balance));
            account = new Credit(UserBO.user.UserID, interest, balance);

            account.AccountType = "Credit account";


            do
            {
                Console.WriteLine("\nType account name");
                account.AccountAlias = Console.ReadLine();
                if (string.IsNullOrEmpty(account.AccountAlias))
                    Console.WriteLine("Error:Name can not be empty");
            } while (string.IsNullOrEmpty(account.AccountAlias));
            try
            {
                Console.WriteLine(((Credit)account).ToString());
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Specified cast is not valid");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }

            //operation
            AddAccount(account, new Operation("New Account", account, 0));
            return account;




        }

        public override bool AddAccount(Account a, Operation operation)
        {
            Credit c = a as Credit;
            

            SqlParameter[] parameters = new SqlParameter[12];
            parameters[0] = new SqlParameter("@alias", c.AccountAlias);
            parameters[1] = new SqlParameter("@type", 3);
            parameters[2] = new SqlParameter("@balance", c.Balance);
            parameters[3] = new SqlParameter("@userID", UserBO.user.UserID);
            parameters[4] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[5] = new SqlParameter("@interest", 20);
            parameters[6] = new SqlParameter("@creditLimit",10000);
            parameters[7] = new SqlParameter("@createdAt", DateTime.Now);

            parameters[8] = new SqlParameter("@operationType", operation.OperationType);
            parameters[9] = new SqlParameter("@amount", operation.Amount);
            parameters[10] = new SqlParameter("@currentBalance", ((Account)operation.Account).Balance);
            //parameters[11] = new SqlParameter("@previousBalance", operation.PreviousBalance);
            parameters[11] = new SqlParameter("@operationDate", operation.Date);


            bool res = new AccountDataAccess().Store(parameters);
            return res;
        }

        public override void UpdateAccount(Account a, Operation operation)
        {
            //throw new NotImplementedException();
            Credit c = a as Credit;

            SqlParameter[] parameters = new SqlParameter[11];
            parameters[1] = new SqlParameter("@alias", c.AccountAlias);
            parameters[2] = new SqlParameter("@balance", c.Balance);
            parameters[3] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[4] = new SqlParameter("@interest", c.Interest);
            parameters[5] = new SqlParameter("@creditLimit", c.Limit);
            parameters[6] = new SqlParameter("@accountID", c.AccountID);
            parameters[0] = new SqlParameter("@userID", UserBO.user.UserID);

            parameters[7] = new SqlParameter("@operationType", operation.OperationType);
            parameters[8] = new SqlParameter("@amount", operation.Amount);
            parameters[9] = new SqlParameter("@currentBalance", ((Account)operation.Account).Balance);
            //parameters[10] = new SqlParameter("@previousBalance", operation.PreviousBalance);
            parameters[10] = new SqlParameter("@operationDate", operation.Date);

            bool res = new AccountDataAccess().Update(parameters);

            if (res)
                Console.WriteLine("Your change is saved credit");
            else
                Console.WriteLine("error credit");
        }
    }
}
