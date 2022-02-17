﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.Account.CurrentAccount
{
    class Current : Account
    {
        private float depositLimit;
        public Current() : base()
        { }
        public Current(int accountID,int userID, string accountName, long accountNumber, string accountType, float balance, float max) : base(accountID,userID, accountName, accountNumber, accountType, balance)
        {
            DepositLimit = max;
        }
        public Current(int userID, float max) : base(userID)
        {
            DepositLimit = max;
        }

        public float DepositLimit { get => depositLimit; set => depositLimit = value; }
    }
}