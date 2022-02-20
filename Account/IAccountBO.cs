using BankingV1._8.Account.Log;
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
        bool AddAccount(Account a, Operation operation);
    }
}
