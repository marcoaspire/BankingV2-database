using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingV1._8.UserFolder
{
    class User
    {
        private string email;
        private string password;
        private string firstName;
        private string lastName;
        private int userID;

        public User()
        {
        }
        public User(string email, string password)
        {
            Email = email;
            Password = password;
            FirstName = "";
            LastName = "";
        }
        public User(string email, string password, string firstName, string lastName)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName=lastName;
        }
        public User(int id,string email, string password, string firstName, string lastName)
        {
            this.userID = id;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }


        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public int UserID { get => userID; }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user.Email.Equals(this.Email) && user.Password.Equals(this.Password))
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return -506688385 + EqualityComparer<string>.Default.GetHashCode(Email);
        }

        public override string ToString()
        {
            return String.Format($"-Welcome dear user, your email is " +
                 $"{this.Email} ");
        }
    }
}
