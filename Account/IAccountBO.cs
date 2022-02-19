using BankingV1._8.Account.Receipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account
{
    interface IAccountBO
    {
        Account NewAccount();
        void DeleteAccount(Account a);
        void UpdateAccount(Account a, Operation operation);
        //Withdraw/Deposit
        bool AddAccount(Account a, Operation operation);
    }
}
