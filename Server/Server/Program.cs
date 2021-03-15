using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml.Serialization;


public class Data
{
    public string name;
    public string message;
}

public class Counter
{
    public static int clientOrder = 0;
}

namespace PlayerData
{
    [System.Serializable]
    public class Broadcast
    {
        public string bc { get; set; }
    }
}

public class Server
{
    static string[] username = { "ilham", "Jalu", "Prakosa" };
    ClientData.SerializableData[] data = new ClientData.SerializableData[100];

    private static void ClientProcess(object argument)
    {
        TcpClient client = (TcpClient)argument;
        Data DataClient = new Data();
        NetworkStream ns = client.GetStream();

        try
        {
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            
            while (true)
            {
                BroadcastMessage(ns, writer);


                DataClient.name = reader.ReadLine();
                Console.WriteLine(" [Name's Client-" + Counter.clientOrder + "]: " + DataClient.name);

                DataClient.message = reader.ReadLine();
                Console.WriteLine(" [Message's Client-" + Counter.clientOrder + "]: " + DataClient.message);

                //ns.Close();

                //var xmlSerializer = new XmlSerializer(typeof(Broadcast));
                //var networkStream = client.GetStream();

                //var a = xmlSerializer.Deserialize(networkStream);


                //var a = new PlayerData.Broadcast().bc;

                //if(a.Equals("broadcast"))
                //    Console.WriteLine(a + "  COK");

                //IFormatter formatter = new BinaryFormatter();
                //stream.Seek(0, SeekOrigin.Begin);
                //b = (Broadcast)formatter.Deserialize(stream);

                writer.WriteLine("[Server has received your message]");
                writer.Flush();

                writer.WriteLine("Your Name: " + DataClient.name);
                writer.Flush();
                
                writer.WriteLine("Your Message: " + DataClient.message);
                writer.Flush();
            }
        }
        catch (IOException)
        {
            Console.WriteLine("\n [has come out]");
        }
        //if (client != null)
        //{
        //    client.Close();
        //}
    }

    public static void BroadcastMessage(Stream s, StreamWriter w)
    {
        ClientData.SerializableData data;
        IFormatter formatter = new BinaryFormatter();
        formatter.Binder = new CustomizedBinder();
        data = (ClientData.SerializableData)formatter.Deserialize(s);

        for(int i = 0; i < username.Length; i++)
        {
            if (data.name == username[i])
            {
                Console.WriteLine(data.name);
                w.WriteLine("Login Sukses");
                w.Flush();
            }
        }

        //BinaryFormatter formatter = new BinaryFormatter();
        //PlayerData.Broadcast b;

        //try
        //{
        //    b = (PlayerData.Broadcast)formatter.Deserialize(s);
        //}
        //catch(Exception e)
        //{
        //    return;
        //}
    }

    public static void Main()
    {
        TcpListener listener = null;

        try
        {
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

                Counter.clientOrder++;
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
