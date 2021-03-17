using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections.Generic;
using ExternalMethods;

namespace CoreProgram
{
    public class GeneralData
    {
        public static int clientOrder = 0;
        public static string clientMessage = "";
    }

    public class ServerSide
    {
        static List<User.Database> ClientDatabase = new List<User.Database>();

        public static void ClientProcess(object argument)
        {
            TcpClient client = (TcpClient)argument;
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            NetworkStream ns = client.GetStream();
            //Data DataClient = new Data();

            LoginPage loginPage = new LoginPage();
            ChatBox chatBox = new ChatBox();
            int playerId = 0;

            try
            {
                while (true)
                {
                    string process = reader.ReadLine();
                    switch (process)
                    {
                        case "1":
                            loginPage.SignIn(ns, writer, ClientDatabase);
                            string uname = loginPage.getterUname();
                            foreach (User.Database ouser in ClientDatabase)
                            {
                                if (uname == ouser.username)
                                    playerId = ouser.identity;
                            }
                            break;

                        case "2":
                            loginPage.SignUp(ns, writer, ClientDatabase);
                            break;

                        case "3":
                            ClientDatabase[playerId].message = chatBox.ReceiveClientMessage(reader);
                            chatBox.BroadcastClientMessage(writer, ClientDatabase[playerId].username + ": " + ClientDatabase[playerId].message);
                            break;

                        case "4":
                            Randomizer.Randomize(ns);
                            break;
                    }

                    process = "0";
                }
            }
            catch (IOException)
            {
                Console.WriteLine("\n [has come out]");
            }
        }

        public static void Main()
        {
            TcpListener listener = null;

            try
            {
                ClientDatabase.Add(new User.Database());
                ClientDatabase[0].setterData(ClientDatabase.Count - 1, "admin", "admin");

                Console.WriteLine(" [SERVER-SIDE]");
                Console.Write(" [Input Your IP Address]: ");
                string ipAddress = Console.ReadLine();

                listener = new TcpListener(IPAddress.Parse(ipAddress), 8080);
                listener.Start();

                Console.WriteLine(" [Starting Program...]");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    //Console.WriteLine(" [There's Client Join...]");
                    Thread newThread = new Thread(ClientProcess);

                    GeneralData.clientOrder++;
                    newThread.Start(client);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }
        }
    }

    sealed class CustomizedBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type returntype = null;
            string sharedAssemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            assemblyName = Assembly.GetExecutingAssembly().FullName;
            typeName = typeName.Replace(sharedAssemblyName, assemblyName);
            returntype =
                    Type.GetType(String.Format("{0}, {1}",
                    typeName, assemblyName));

            return returntype;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            base.BindToName(serializedType, out assemblyName, out typeName);
            assemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }
    }
}