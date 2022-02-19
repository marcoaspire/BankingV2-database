using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account
{
    abstract class Account : ICloneable
    {
        private int accountID;
        //private long accountNumber;
        private string accountAlias;
        private string accountType;
        protected float balance;
        private int userID;
        private DateTime createdAt;
        //constructors
        public Account()
        {
            this.createdAt = DateTime.Now;
        }
        public Account(int userID)
        {
            this.createdAt = DateTime.Now;
            this.userID = userID;
        }
        public Account(int accountID,int userID, string accountName, string accountType, float balance)
        {
            AccountAlias = accountName;
            AccountType = accountType;
            Balance = balance;
            this.createdAt = DateTime.Now;
            this.userID = userID;
            this.accountID = accountID;
        }
        public Account(int accountID, string accountName, string accountType, float balance)
        {

            this.accountID = accountID;
            AccountAlias = accountName;
            AccountType = accountType;
            Balance = balance;
            this.createdAt = DateTime.Now;
        }
        

        //Properties 
        //public long AccountNumber { get => accountNumber; set => accountNumber = value; }
        public string AccountAlias { get => accountAlias; set => accountAlias = value; }
        public string AccountType { get => accountType; set => accountType = value; }
        public float Balance { get => balance; set => balance = value; }
        public DateTime CreatedAt
        {
            get => createdAt;
            //set => createdAt = value; // read only
        }
        public int UserID { get => userID; //set => owner = value;
                                             }
        public int AccountID { get => accountID;  }


        //methods


        public override string ToString()
        {
            return String.Format($"-Hello dear user, your " +
                $"{this.AccountType} {this.AccountAlias}, the account number is {this.AccountID} and you have ${this.Balance}." +
                $"It was opened on {this.CreatedAt}");

        }

        public override bool Equals(Object obj)
        {
            try
            {
                Account account = obj as Account;
                return this.accountID.Equals(account.accountID);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Specified cast is not valid");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }
            return false;
        }

        public object Clone()
        {
            return (Account)MemberwiseClone();
        }

        public override int GetHashCode()
        {
            return 239528309 + accountID.GetHashCode();
        }
    }
}
