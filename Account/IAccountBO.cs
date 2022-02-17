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
        void RemoveAccount(Account a);
        void UpdateAccount(Account a);

    }
}
