using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace ExternalMethods
{
    class LoginPage
    {
        public static string playerUname;

        public string getterUname()
        {
            return playerUname;
        }

        public void SignIn(Stream s, StreamWriter writer, List<User.Database> ClientDatabase)
        {
            IFormatter formatter_recv = new BinaryFormatter();
            formatter_recv.Binder = new CoreProgram.CustomizedBinder();
            User.LoginData data_recv = (User.LoginData)formatter_recv.Deserialize(s);
            bool found = false;

            foreach (User.Database ouser in ClientDatabase)
            {
                if (data_recv.name == ouser.username)
                {
                    found = true;
                    if (data_recv.pass == ouser.password)
                    {
                        playerUname = ouser.username;
                        Console.WriteLine(" " + ouser.username + " signed in");
                        writer.WriteLine("login was successful");
                        writer.Flush();

                        IFormatter formatter_send = new BinaryFormatter();
                        formatter_send.Binder = new CoreProgram.CustomizedBinder();
                        User.LoginData data_send = new User.LoginData(data_recv.name, data_recv.pass);
                        formatter_send.Serialize(s, data_send);
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

            //PrintDatabase(ClientDatabase);
        }
        public void SignUp(Stream s, StreamWriter writer, List<User.Database> ClientDatabase)
        {
            IFormatter formatter_recv = new BinaryFormatter();
            formatter_recv.Binder = new CoreProgram.CustomizedBinder();
            User.LoginData data_recv = (User.LoginData)formatter_recv.Deserialize(s);
            bool found = false;

            for (int i = 0; i < ClientDatabase.Count; i++)
            {
                if (data_recv.name == ClientDatabase[i].username)
                {
                    found = true;
                    writer.WriteLine("account failed to register! change your username!");
                    writer.Flush();
                }
            }

            if (!found)
            {
                ClientDatabase.Add(new User.Database());
                ClientDatabase[ClientDatabase.Count - 1].setterData(ClientDatabase.Count - 1, data_recv.name, data_recv.pass);
                writer.WriteLine("account successfully registered");
                writer.Flush();
            }

            //PrintDatabase(ClientDatabase);
        }

        public void PrintDatabase(List<User.Database> ClientDatabase)
        {
            //Console.Clear();
            Console.WriteLine(" [SERVER-SIDE]");
            foreach (User.Database oclient in ClientDatabase)
            {
                Console.WriteLine(" id: " + oclient.identity +
                                  ", username: " + oclient.username +
                                  ", password: " + oclient.password);
            }
        }
    }
}
