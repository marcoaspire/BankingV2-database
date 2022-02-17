﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account.CreditAccount
{
    class Credit : Account
    {
        private float interest;
        private float limit;
        public Credit() : base()
        { }
        public Credit(int accountID,int userID, string accountName, long accountNumber, string accountType, float limit, float interest) : base(accountID,userID, accountName, accountNumber, accountType, 0)
        {
            Interest = interest;
            Limit = limit;
            Balance = 0;
        }

        public Credit(int accountID,int userID, string accountName, long accountNumber, string accountType, float balance, float limit, float interest) : base(accountID,userID, accountName, accountNumber, accountType, 0)
        {
            Interest = interest;
            Limit = limit;
            Balance = balance;
        }


        public Credit(int userID, float interest, float limit) : base(userID)
        {
            Interest = interest;
            Limit = limit;
            Balance = 0;
        }

        //Properties 
        public float Interest { get => interest; set => interest = value; }
        public float Limit { get => limit; set => limit = value; }


        public override string ToString()
        {
            return String.Format($"-Hello dear user, your " +
                $"{this.AccountType} {this.AccountAlias}, the account number is {this.AccountNumber}, it was opened on { this.CreatedAt}.\n" +
                $"Balance: {this.Balance} \n" +
                $"Available Credit: {this.Limit - this.Balance} \n" +
                $"Credit Limit: {this.Limit} \n");
        }






    }
}
