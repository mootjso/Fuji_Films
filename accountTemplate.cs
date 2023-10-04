using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logger
{
    public class Account
    {
        public string Username;
        public string Password;
        public string Email;
        public string Number;

        public Account(string username, string password, string email, string number)
        {
            Username = username;
            Password = password;
            Email = email;
            Number = number;
        }
    }
    public class Admin
    {
        public string Username;
        public string Password;

        public Admin(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
