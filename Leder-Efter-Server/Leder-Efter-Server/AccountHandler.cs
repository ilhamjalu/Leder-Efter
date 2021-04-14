using System;
using System.Collections.Generic;
using System.Text;

namespace Leder_Efter_Server
{
    class AccountDatabase
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool active { get; set; }

        public AccountDatabase(string uname, string pass, bool on)
        {
            username = uname;
            password = pass;
            active = on;
        }
    }

    class AccountHandler
    {
        public static string SignIn(string uname, string pass)
        {
            foreach (AccountDatabase oacc in Server.accountDatabase)
            {
                if (uname == oacc.username && !oacc.active)
                {
                    if (pass == oacc.password)
                    {
                        oacc.active = true;
                        Console.WriteLine($"There's player signIn: {uname}");
                        return "login was successful";
                    }
                    else
                    {
                        return "login failed! your password is wrong";
                    }
                }
                else if (uname == oacc.username && oacc.active)
                {
                    return "login failed! another user is using your account";
                }
            }

            return "login failed! your account's not found";
        }

        public static string SignUp(string uname, string pass)
        {
            foreach (AccountDatabase oacc in Server.accountDatabase)
            {
                if (uname == oacc.username)
                {
                    return "login failed! change your username";
                }
            }

            Server.accountDatabase.Add(new AccountDatabase(uname, pass, false));
            Console.WriteLine($"There's player join: {uname}");
            return "your account registered successfully";
        }
    }
}
