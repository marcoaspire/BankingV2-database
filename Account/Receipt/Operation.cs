using BankingV1._8.Account.CreditAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account.Receipt
{
    class Operation
    {
        private string operationType;
        private Account account;
        private float amount;
        private float previousBalance;
        private DateTime date;
        public Operation()
        { Date = DateTime.Now; }
        public Operation(string operationType, Account account, float previousBalance, float amount)
        {
            OperationType = operationType;
            Account = account;
            PreviousBalance = previousBalance;
            Amount = amount;
            Date = DateTime.Now;
        }

        public DateTime Date { get => date; set => date = value; }
    
        public string OperationType { get => operationType; set => operationType = value; }
        public float PreviousBalance { get => previousBalance; set => previousBalance = value; }
        public float Amount { get => amount; set => amount = value; }
        internal Account Account { get => account; set => account = value; }


        public override string ToString()
        {
            if (this.Account.AccountType.Equals("Credit account"))
            {
                Credit credit = (Credit)this.Account;
                Console.WriteLine("Entree {0}- {1}", credit.Limit, credit.Balance);
                return String.Format("{0}\t\t{1}\t{2}\t\t{3}\t\t{4}\t\t{5}", this.OperationType, this.Account.AccountNumber,
                this.Account.AccountType, this.PreviousBalance, credit.Limit - credit.Balance, this.Amount);
            }
            else
                return String.Format("{0}\t\t{1}\t{2}\t\t{3}\t\t{4}\t\t{5}", this.OperationType, this.Account.AccountNumber,
                this.Account.AccountType, this.PreviousBalance, this.Account.Balance, this.Amount);
        }
    }
}
