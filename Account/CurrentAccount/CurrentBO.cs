﻿using BankingV1._8.Account.Receipt;
using BankingV1._8.Menu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account.CurrentAccount
{
    class CurrentBO : AccountBO
    {

        public override Account NewAccount()
        {
            bool validBalance = false;
            float balance;

            Current account = new Current(BankMenu.userID, 10000);
            account.AccountType = "Current account";

            do
            {
                Console.WriteLine("\nType account name");
                account.AccountAlias = Console.ReadLine();
                if (string.IsNullOrEmpty(account.AccountAlias))
                    Console.WriteLine("Error:Name can not be empty");
            } while (string.IsNullOrEmpty(account.AccountAlias));

            do
            {
                Console.WriteLine("Type the balance");
                validBalance = float.TryParse(Console.ReadLine(), out balance);
                if (CheckBalance(balance))
                    account.Balance = balance;
            } while (!validBalance || account.Balance <= 0);
            //Console.WriteLine($"Hello dear user, your have a new {account.AccountType} {account.AccountAlias}, the account number is {account.AccountNumber} and you have ${account.Balance}");

            //operation
            AddAccount(account, new Operation("New Account", account, 0));
            //new OperationBO().AddOperation(new Operation("New Account", account, 0, 0));

            return account;
        }


        /*
        public override void Deposit(Account account)
        {
            Current currentAccount =account as Current;
            bool validDeposit = false;
            float deposit;
            try
            {
                do
                {
                    Console.WriteLine("Type the amount you want to deposit");
                    validDeposit = float.TryParse(Console.ReadLine(), out deposit);
                } while (!validDeposit || deposit < 0);
                if (deposit > currentAccount.DepositLimit)
                    throw new Exception("Deposit can be greater than its limit " + currentAccount.DepositLimit);
                currentAccount.Balance += deposit;
                //OperationBO.operations.Add(DateTime.Now, new Operation("Deposit", (Account)account.Value.Clone(), currentAccount.Balance, deposit));


                this.UpdateAccount(currentAccount);

                Console.WriteLine($"Now your balance is ${account.Balance}");

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
        **/
        public override bool AddAccount(Account a, Operation operation)
        {
            Current c = a as Current;
            SqlParameter[] parameters = new SqlParameter[12];
            parameters[0] = new SqlParameter("@alias", c.AccountAlias);
            parameters[1] = new SqlParameter("@type", 1);
            parameters[2] = new SqlParameter("@balance", c.Balance);
            parameters[3] = new SqlParameter("@userID", BankMenu.userID);
            parameters[4] = new SqlParameter("@depositLimit", c.DepositLimit);
            parameters[5] = new SqlParameter("@interest", DBNull.Value);
            parameters[6] = new SqlParameter("@creditLimit", DBNull.Value);
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
            Current c = a as Current;
                        
            SqlParameter[] parameters = new SqlParameter[11];
            parameters[1] = new SqlParameter("@alias", c.AccountAlias);
            parameters[2] = new SqlParameter("@balance", c.Balance);
            parameters[3] = new SqlParameter("@depositLimit", c.DepositLimit);
            parameters[4] = new SqlParameter("@interest", DBNull.Value);
            parameters[5] = new SqlParameter("@creditLimit", DBNull.Value);
            parameters[6] = new SqlParameter("@accountID", c.AccountID);
            parameters[0] = new SqlParameter("@userID", BankMenu.userID);

            parameters[7] = new SqlParameter("@operationType", operation.OperationType);
            parameters[8] = new SqlParameter("@amount", operation.Amount);
            parameters[9] = new SqlParameter("@currentBalance", ((Account)operation.Account).Balance);
            //parameters[10] = new SqlParameter("@previousBalance", operation.PreviousBalance);
            parameters[10] = new SqlParameter("@operationDate", operation.Date);

            bool res = new AccountDataAccess().Update(parameters);

            if (res)
                Console.WriteLine("Your change is saved"); 
            else
                Console.WriteLine("error al cambiar");
            

        }


    }
}
