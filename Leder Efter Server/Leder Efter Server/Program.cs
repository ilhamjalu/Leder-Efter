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
        static List<User.Database> ClientAccount = new List<User.Database>();
        static List<User.Database> ClientConnected = new List<User.Database>();
        static List<User.FPSData> ClientFPS = new List<User.FPSData>();
        
        public static void ClientProcess(object argument)
        {
            TcpClient client = (TcpClient)argument;
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            NetworkStream ns = client.GetStream();

            LoginPage loginPage = new LoginPage();
            ChatBox chatBox = new ChatBox();
            FPSPage fps = new FPSPage();
            Randomizer random = new Randomizer();
            int playerId = GeneralData.clientOrder - 1;

            try
            {
                while (true)
                {
                    string process = reader.ReadLine();

                    writer.WriteLine(process);
                    writer.Flush();

                    switch (process)
                    {
                        case "1":
                            loginPage.SignIn(ns, writer, ClientAccount);
                            
                            ClientConnected.Add(new User.Database());
                            ClientConnected[ClientConnected.Count - 1].setterData(playerId, loginPage.Getter(0), loginPage.Getter(1));

                            ClientFPS.Add(new User.FPSData());
                            ClientFPS[ClientFPS.Count - 1].setterData(loginPage.Getter(0), 100, 0, false, false, false);

                            loginPage.SetterId(playerId);
                            break;

                        case "2":
                            loginPage.SignUp(ns, writer, ClientAccount);
                            break;

                        case "3":
                            string msg = chatBox.ReceiveClientMessage(reader);
                            chatBox.BroadcastClientMessage(writer, ClientConnected[playerId].username + ": " + msg);
                            break;

                        case "4":
                            random.Randomize(ns, writer);
                            break;

                        case "5":
                            fps.FPSDataReceiver(ns, ClientConnected, ClientFPS, playerId);

                            writer.WriteLine(ClientFPS.Count.ToString());
                            writer.Flush();

                            fps.FPSDataBroadcast(ns, ClientFPS);
                            break;
                    }

                    process = "";
                }
            }
            catch (IOException)
            {
                Console.WriteLine("\n [" + ClientConnected[playerId].username + " has come out]");
            }
        }

        public static void Main()
        {
            TcpListener listener = null;

            try
            {
                ClientAccount.Add(new User.Database());
                ClientAccount[ClientAccount.Count - 1].setterData(ClientAccount.Count - 1, "123", "123");
                ClientAccount.Add(new User.Database());
                ClientAccount[ClientAccount.Count - 1].setterData(ClientAccount.Count - 1, "111", "111");

                Console.WriteLine(" [SERVER-SIDE]");
                Console.WriteLine(" [Starting Program...]");

                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine(" [There's Client Join...]");
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