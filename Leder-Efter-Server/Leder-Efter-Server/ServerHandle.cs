using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Leder_Efter_Server
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
        }

        public static void SignInReceived(int _fromClient, Packet _packet)
        {
            ClientData.Account _account = _packet.ReadObject<ClientData.Account>();

            string validation = AccountHandler.SignIn(_account.username, _account.password);
            if (validation == "login was successful")
                Server.readyDatabase.Add(new ReadyDatabase(_fromClient, false));

            ServerSend.SignInValidation(_fromClient, validation);
        }

        public static void SignUpReceived(int _fromClient, Packet _packet)
        {
            ClientData.Account _account = _packet.ReadObject<ClientData.Account>();
            string validation = AccountHandler.SignUp(_account.username, _account.password);
            ServerSend.SignUpValidation(_fromClient, validation);
        }

        public static void ChatboxReceived(int _fromClient, Packet _packet)
        {
            ClientData.Chatbox _chatbox = _packet.ReadObject<ClientData.Chatbox>();
            ServerSend.BroadcastChatbox(_chatbox.username, _chatbox.message);
        }

        public static void RandomizeReceived(int _fromClient, Packet _packet)
        {
            bool _ready = _packet.ReadBool();
            ReadyHandler.ReadySetter(_fromClient, _ready);

            if (_ready)
            {
                ReadyHandler.totalReady++;
                ServerSend.BroadcastReady(ReadyHandler.totalReady, Server.readyDatabase.Count, "(wait)", "(wait)");

                if (ReadyHandler.totalReady == Server.readyDatabase.Count)
                {
                    Console.WriteLine($"All player're ready: {RandomizeHandler.StuffRandomizer()} & {RandomizeHandler.ColorRandomizer()}");
                    ServerSend.BroadcastReady(ReadyHandler.totalReady, Server.readyDatabase.Count, RandomizeHandler.StuffRandomizer(), RandomizeHandler.ColorRandomizer());
                }
            }
        }
    }
}
