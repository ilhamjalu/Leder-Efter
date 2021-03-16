using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections.Generic;

namespace ExternalMethods
{
    class ChatBox
    {
        public static string message;

        public string ReceiveClientMessage(StreamReader reader)
        {
            return message = reader.ReadLine();
        }

        public void BroadcastClientMessage(StreamWriter writer, string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }
    }
}
