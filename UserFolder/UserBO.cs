using BankingV1._8.Account;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankingV1._8.UserFolder
{
    class UserBO
    {
        //it´s like a session variable
        public static User user = null; //change to User class

        static UserBO()
        {
            user = new User();
        }

        
        public static bool ValidateName(string name)
        {
            return (Regex.IsMatch(name, @"^[a-zA-Z]+$") && name.Length>1);
        }



        public static bool ValidatePassword(string password)
        {
            if (password.Length < 8 || password.Contains(" ") || !password.Any(char.IsLetterOrDigit) || !password.Any(char.IsDigit)
                 || !password.Any(char.IsLower) || !password.Any(char.IsUpper))
                return false;
            return true;
        }
        public bool AddUser(User u)
        {

            SqlParameter[] parameter = new SqlParameter[5];
            parameter[0] = new SqlParameter("@email",  u.Email);
            parameter[1] = new SqlParameter("@password", u.Password);
            parameter[2] = new SqlParameter("@firstname", u.FirstName);
            parameter[3] = new SqlParameter("@lastname", u.LastName);
            parameter[4] = new SqlParameter("@res", -1);
            bool res = new UserDataAccess().Store(parameter);
            return res;
        }

        public User Find(User u)
        {

            SqlParameter[] parameter = new SqlParameter[4];
            parameter[0] = new SqlParameter("@email", u.Email);
            parameter[1] = new SqlParameter("@password", u.Password);
            parameter[2] = new SqlParameter("@firstname", u.FirstName);
            parameter[3] = new SqlParameter("@lastname", u.LastName);

            User res = new UserDataAccess().Search(parameter);
            return res;
        }

        public User FindByID(int id)
        {
            SqlParameter parameter;
            parameter = new SqlParameter("@userID", id);
            
            User res = new UserDataAccess().SearchByID(parameter);
            return res;
        }




    }
}
