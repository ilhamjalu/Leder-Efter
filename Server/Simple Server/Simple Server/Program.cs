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
        static ClientData.Player player;
        static List<Database.Client> ClientDatabase = new List<Database.Client>();
        static int playerId;
        static string[] warna = {"hijau", "merah", "ungu"};
        static string[] item = { "Apel", "Anggur", "Jeruk" };

        public static void ClientProcess(object argument)
        {
            TcpClient client = (TcpClient)argument;
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            NetworkStream ns = client.GetStream();
            //Data DataClient = new Data();

            ExternalMethods.LoginPage loginPage = new LoginPage();
            ExternalMethods.ChatBox chatBox = new ChatBox();

            try
            {
                while (true)
                {
                    string process = reader.ReadLine();

                    if (process == "0")
                    {
                        loginPage.SignIn(ns, writer, ClientDatabase);
                        playerId = loginPage.getterId();
                    }
                    else if (process == "1")
                    {
                        loginPage.SignUp(ns, writer, ClientDatabase);
                    }
                    else if (process == "3")
                    {
                        ClientDatabase[playerId].message = chatBox.ReceiveClientMessage(reader);
                        Console.WriteLine(" " + ClientDatabase[playerId].username + ": " + ClientDatabase[playerId].message);
                        chatBox.BroadcastClientMessage(writer, ClientDatabase[playerId].username + ": " + ClientDatabase[playerId].message);
                    }
                    else if (process == "4")
                    {
                        writer.WriteLine("ok");
                        writer.Flush();
                        Randomize(ns);
                        Console.WriteLine("ACAK");
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("\n [has come out]");
            }
        }

        public static void Randomize(Stream s)
        {
            Random random2 = new Random();
            Random random = new Random();
            int a = random.Next(warna.Length);
            int b = random.Next(item.Length);
            
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new CustomizedBinder();
            player = new ClientData.Player(warna[a], item[b]);
            formatter.Serialize(s, player);

            Console.WriteLine(player.item);
            Console.WriteLine(player.team);
        }

        public static void Main()
        {
            TcpListener listener = null;
            
            try
            {
                ClientDatabase.Add(new Database.Client());
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