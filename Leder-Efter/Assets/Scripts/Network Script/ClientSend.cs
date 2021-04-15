using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeRequest()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeRequest))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SignInRequest(string uname, string pass)
    {
        using (Packet _packet = new Packet((int)ClientPackets.signInRequest))
        {
            ClientData.Account _account = new ClientData.Account(uname, pass);

            _packet.Write<ClientData.Account>(_account);
            SendTCPData(_packet);
        }
    }

    public static void SignUpRequest(string uname, string pass)
    {
        using (Packet _packet = new Packet((int)ClientPackets.signUpRequest))
        {
            ClientData.Account _account = new ClientData.Account(uname, pass);

            _packet.Write<ClientData.Account>(_account);
            SendTCPData(_packet);
        }
    }

    public static void ChatboxRequest(string uname, string msg)
    {
        using (Packet _packet = new Packet((int)ClientPackets.chatboxRequest))
        {
            ClientData.Chatbox _chatbox = new ClientData.Chatbox(uname, msg);

            _packet.Write<ClientData.Chatbox>(_chatbox);
            SendTCPData(_packet);

            Debug.Log($"Your message: {msg}");
        }
    }

    public static void RandomizeRequest()
    {
        using (Packet _packet = new Packet((int)ClientPackets.randomizeRequest))
        {
            _packet.Write(true);
            SendTCPData(_packet);
        }
    }
    #endregion
}