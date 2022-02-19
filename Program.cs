using BankingV1._8.Account;
using BankingV1._8.Menu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8
{
    
    class Program
    {
        static void Main(string[] args)
        {
            BankMenu bank = new BankMenu();
            //FileBO.UsersLoad();
            bank.LoginMenu();

            /*
                AccountDataAccess dataAccess = new AccountDataAccess();
                DataSet ds =dataAccess.GetAllAccountsFromDB();
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Console.WriteLine(item["AccountNumber"].ToString());
                    //Department dept = new Department(Convert.ToInt32(item["deptno"].ToString()), item["dname"].ToString(), item["loc"].ToString());
                    //list.Add(dept);
                }
            */
        }
    }
}
