using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account.SavingAccount
{
    class Saving : Account
    {
        private float interest;
        public Saving() : base()
        { }
        public Saving(int accountID,int userID, string accountName, string accountType, float balance, float interest) : base(accountID,userID, accountName, accountType, balance)
        {
            Interest = interest;
        }
        public Saving(int userID, float interest) : base(userID)
        {
            Interest = interest;

        }
        public Saving(int accountID,string accountName, string accountType, float balance, float interest) : base(accountID,accountName, accountType, balance)
        {
            Interest = interest;
        }
        //Properties 
        public float Interest { get => interest; set => interest = value; }

        public override string ToString()
        {
            return String.Format($"-Hello dear user, your " +
                $"{this.AccountType} {this.AccountAlias}, the account number is {this.AccountID}, it was opened on { this.CreatedAt}.\n" +
                $"Balance: {this.Balance} \n" +
                $"interest rate: {this.Interest} %\n");
        }
    }
}
