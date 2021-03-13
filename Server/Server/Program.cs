using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;

public class Counter
{
    public static int clientOrder = 0;
}

public class Server
{
    ClientData.SerializableData[] data = new ClientData.SerializableData[100];

    private static void ClientProcess(object argument)
    {
        TcpClient client = (TcpClient)argument;

        try
        {
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());

            while (true)
            {
                
            }
        }
        catch (IOException)
        {
            Console.WriteLine("\n [has come out]");
        }
        if (client != null)
        {
            client.Close();
        }
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