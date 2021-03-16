using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace ExternalMethods
{
    class LoginPage
    {
        public static int playerOrder;

        public int getterId()
        {
            return playerOrder;
        }

        public void SignIn(Stream s, StreamWriter writer, List<Database.Client> ClientDatabase)
        {
            ClientData.SerializableData data;
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new CoreProgram.CustomizedBinder();
            data = (ClientData.SerializableData)formatter.Deserialize(s);
            bool found = false;

            for (int i = 0; i < ClientDatabase.Count; i++)
            {
                if (data.name == ClientDatabase[i].username)
                {
                    found = true;
                    if (data.pass == ClientDatabase[i].password)
                    {
                        playerOrder = i;

                        writer.WriteLine("login was successful");
                        writer.Flush();

                        writer.WriteLine(Convert.ToString(ClientDatabase[i].identity));
                        writer.Flush();

                        writer.WriteLine(ClientDatabase[i].username);
                        writer.Flush();

                        writer.WriteLine(ClientDatabase[i].password);
                        writer.Flush();
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

            PrintDatabase(ClientDatabase);
        }
        public void SignUp(Stream s, StreamWriter writer, List<Database.Client> ClientDatabase)
        {
            ClientData.SerializableData data;
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new CoreProgram.CustomizedBinder();
            data = (ClientData.SerializableData)formatter.Deserialize(s);
            bool found = false;

            for (int i = 0; i < ClientDatabase.Count; i++)
            {
                if (data.name == ClientDatabase[i].username)
                {
                    found = true;
                    writer.WriteLine("account failed to register! change your username!");
                    writer.Flush();
                }
            }

            if (!found)
            {
                ClientDatabase.Add(new Database.Client());
                ClientDatabase[ClientDatabase.Count - 1].setterData(ClientDatabase.Count - 1, data.name, data.pass);
                writer.WriteLine("account successfully registered");
                writer.Flush();
            }

            PrintDatabase(ClientDatabase);
        }

        public void PrintDatabase(List<Database.Client> ClientDatabase)
        {
            Console.Clear();
            Console.WriteLine(" [SERVER-SIDE]");
            foreach (Database.Client oclient in ClientDatabase)
            {
                Console.WriteLine(" id: " + oclient.identity +
                                  ", username: " + oclient.username +
                                  ", password: " + oclient.password);
            }
        }
    }
}
