﻿using System;
using System.Collections.Generic;
using System.Text;


namespace Leder_Efter_Server
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                    Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                    Server.clients[i].udp.SendData(_packet);
            }
        }

        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void SignInValidation(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.signIn))
            {
                _packet.Write(_msg);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void SignUpValidation(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.signUp))
            {
                _packet.Write(_msg);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void BroadcastChatbox(string _uname, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.chatbox))
            {
                ClientData.Chatbox _chatbox = new ClientData.Chatbox(_uname, _msg);

                _packet.Write<ClientData.Chatbox>(_chatbox);
                SendTCPDataToAll(_packet);
            }
        }

        public static void BroadcastReady(int totalReady, int totalPlayer, string stuff, string color)
        {
            using (Packet _packet = new Packet((int)ServerPackets.randomize))
            {
                _packet.Write(totalReady);
                _packet.Write(totalPlayer);

                if (totalReady == totalPlayer)
                {
                    _packet.Write(stuff);
                    _packet.Write(color);
                }

                SendTCPDataToAll(_packet);
            }
        }

        public static void Siap(int totalReady, int totalPlayer, string kondisi)
        {
            using (Packet _packet = new Packet((int)ServerPackets.readyGaQi))
            {
                _packet.Write(totalReady);
                _packet.Write(totalPlayer);

                if (totalReady == totalPlayer)
                {
                    _packet.Write(kondisi);
                }

                SendTCPDataToAll(_packet);
            }
        }

        public static void SendColor()
        {
            using (Packet _packet = new Packet((int)ServerPackets.color))
            {
                var a = RandomizeHandler.ColorRandomizer();
                
                _packet.Write(a); 
                SendTCPDataToAll(_packet);
            }
        }

        public static void PositionBroadcast(int _toClient, ClientData.Position pos)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(pos.id);
                _packet.Write(pos.username);
                _packet.Write(pos.position);
                _packet.Write(pos.rotation);

                SendTCPData(_toClient, _packet);
                //SendTCPDataToAll(_packet);
            }
        }

        public static void PlayerPos(ClientData.Position pos)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPos))
            {
                _packet.Write(pos.id);
                _packet.Write(pos.position);

                SendUDPDataToAll(_packet);
            }
        }

        public static void PlayerRot(ClientData.Position pos)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRot))
            {
                _packet.Write(pos.id);
                _packet.Write(pos.rotation);

                SendUDPDataToAll(pos.id, _packet);
            }
        }
        #endregion
    }
}
