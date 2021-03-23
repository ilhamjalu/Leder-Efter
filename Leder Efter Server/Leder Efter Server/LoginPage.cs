using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace ExternalMethods
{
    class LoginPage
    {
        static int playerId = 0;
        public static string username { get; set; }
        public static string password { get; set; }

        public void SetterId(int id)
        {
            playerId = id;
        }

        public string Getter(int option)
        {
            if (option == 0)
                return username;
            else if (option == 1)
                return password;

            return "";
        }

        public void SignIn(Stream s, StreamWriter writer, List<User.Database> ClientAccount)
        {
            bool found = false;
            IFormatter formatter_recv = new BinaryFormatter();
            formatter_recv.Binder = new CoreProgram.CustomizedBinder();
            User.LoginData data_recv = (User.LoginData)formatter_recv.Deserialize(s);

            foreach (User.Database ouser in ClientAccount)
            {
                if (data_recv.username == ouser.username)
                {
                    found = true;
                    if (data_recv.password == ouser.password)
                    {
                        writer.WriteLine("login was successful");
                        writer.Flush();

                        IFormatter formatter_send = new BinaryFormatter();
                        formatter_send.Binder = new CoreProgram.CustomizedBinder();
                        User.LoginData data_send = new User.LoginData(playerId, data_recv.username, data_recv.password);
                        formatter_send.Serialize(s, data_send);

                        username = data_recv.username;
                        password = data_recv.password;

                        Console.WriteLine(" " + ouser.username + " has signed in");
                        return;
                    }
                    else
                    {
                        writer.WriteLine("login failed! the password is wrong");
                        writer.Flush();
                    }
                }
            }

            if (!found)
            {
                writer.WriteLine("login failed! account not found");
                writer.Flush();
            }
        }
        public void SignUp(Stream s, StreamWriter writer, List<User.Database> ClientAccount)
        {
            IFormatter formatter_recv = new BinaryFormatter();
            formatter_recv.Binder = new CoreProgram.CustomizedBinder();
            User.LoginData data_recv = (User.LoginData)formatter_recv.Deserialize(s);
            bool found = false;

            foreach (User.Database ouser in ClientAccount)
            {
                if (data_recv.username == ouser.username)
                {
                    found = true;
                    writer.WriteLine("login failed! change your username");
                    writer.Flush();
                    break;
                }
            }

            if (!found)
            {
                ClientAccount.Add(new User.Database());
                ClientAccount[ClientAccount.Count - 1].setterData(ClientAccount.Count - 1, data_recv.username, data_recv.password);

                writer.WriteLine("account successfully registered");
                writer.Flush();

                Console.WriteLine(" " + data_recv.username + " has signed up");
            }
        }
    }
}
