using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Data
{
    public string name;
    public string message;
}

public class Counter
{
    public static int clientOrder = 0;
}

public class Server
{
    private static void ClientProcess(object argument)
    {
        TcpClient client = (TcpClient)argument;
        Data DataClient = new Data();

        try
        {
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());

            while (true)
            {
                DataClient.name = reader.ReadLine();
                Console.WriteLine(" [Name's Client-" + Counter.clientOrder + "]: " + DataClient.name);

                DataClient.message = reader.ReadLine();
                Console.WriteLine(" [Message's Client-" + Counter.clientOrder + "]: " + DataClient.message);

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
            Console.WriteLine("\n [" + DataClient.name + " has come out]");
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
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
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