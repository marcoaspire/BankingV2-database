using BankingV1._8.Account.Receipt;
using BankingV1._8.Menu;
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
        public override float MonthEndBalance(Account account)
        {
            //charge interest
            try
            {
                Credit creditAccount = (Credit)account;

                if (creditAccount.Balance > 0)
                    return creditAccount.Balance + creditAccount.Balance * creditAccount.Interest / (100 * 12);
                else
                    return 0;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Specified cast is not valid");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return 0;
        }
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

                    account.Balance += withdrawal;
                    UpdateAccount(account);
                    //OperationBO.operations.Add(DateTime.Now, new Operation("Pay with credit", (Account)account.Value.Clone(), availableCredit, withdrawal));
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
                account.Balance -= deposit;
                UpdateAccount(account);
                //OperationBO.operations.Add(DateTime.Now, new Operation("Credit payment", (Account)account.Value.Clone(), creditAccount.Balance, deposit));
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
            bool validAccount, validBalance = false;
            long accountNumber;
            float balance, interest = 30;

            Credit account = null;
            do
            {
                Console.WriteLine("\nHow much do you want to deposit? Your deposit will be equal to your credit limit");
                validBalance = Single.TryParse(Console.ReadLine(), out balance);

            } while (!validBalance || !CheckBalance(balance));
            account = new Credit(BankMenu.userID, interest, balance);

            account.AccountType = "Credit account";


            do
            {
                Console.WriteLine("Type your account numbers");
                validAccount = Int64.TryParse(Console.ReadLine(), out accountNumber);
                account.AccountNumber = accountNumber;

            } while (!validAccount);

            do
            {
                Console.WriteLine("Type account name");
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
            AddAccount(account, new Operation("New Account", account, 0, 0));
            return account;




        }

        public override bool AddAccount(Account a, Operation operation)
        {
            Credit c = a as Credit;
            

            SqlParameter[] parameters = new SqlParameter[14];
            parameters[0] = new SqlParameter("@number", c.AccountNumber);
            parameters[1] = new SqlParameter("@alias", c.AccountAlias);
            parameters[2] = new SqlParameter("@type", 3);
            parameters[3] = new SqlParameter("@balance", c.Balance);
            parameters[4] = new SqlParameter("@userID", BankMenu.userID);
            parameters[5] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[6] = new SqlParameter("@interest", 20);
            parameters[7] = new SqlParameter("@creditLimit",10000);
            parameters[8] = new SqlParameter("@createdAt", DateTime.Now);

            parameters[9] = new SqlParameter("@operationType", operation.OperationType);
            parameters[10] = new SqlParameter("@amount", operation.Amount);
            parameters[11] = new SqlParameter("@currentBalance", ((Account)operation.Account).Balance);
            parameters[12] = new SqlParameter("@previousBalance", operation.PreviousBalance);
            parameters[13] = new SqlParameter("@operationDate", operation.Date);


            bool res = new AccountDataAccess().Store(parameters);
            return res;
        }

        public override void UpdateAccount(Account a)
        {
            //throw new NotImplementedException();
            Credit c = a as Credit;

            SqlParameter[] parameters = new SqlParameter[8];
            parameters[0] = new SqlParameter("@number", c.AccountNumber);
            parameters[1] = new SqlParameter("@alias", c.AccountAlias);
            parameters[2] = new SqlParameter("@balance", c.Balance);
            parameters[3] = new SqlParameter("@depositLimit", DBNull.Value);
            parameters[4] = new SqlParameter("@interest", c.Interest);
            parameters[5] = new SqlParameter("@creditLimit", c.Limit);
            parameters[6] = new SqlParameter("@accountID", c.AccountID);
            parameters[7] = new SqlParameter("@userID", BankMenu.userID);


            bool res = new AccountDataAccess().Update(parameters);

            if (res)
                Console.WriteLine("Your change is saved credit");
            else
                Console.WriteLine("error credit");
        }
    }
}
