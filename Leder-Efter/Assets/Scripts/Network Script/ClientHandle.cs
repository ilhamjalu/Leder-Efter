using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeRequest();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SignInValidation(Packet _packet)
    {
        string _msg = _packet.ReadString();
        UILoginManager.instance.notif.text = _msg;

        if (_msg == "login was successful")
            Client.instance.isLogin = 1;
    }

    public static void SignUpValidation(Packet _packet)
    {
        string _msg = _packet.ReadString();
        UILoginManager.instance.notif.text = _msg;
    }

    public static void ChatboxValidation(Packet _packet)
    {
        ClientData.Chatbox _chatbox = _packet.ReadObject<ClientData.Chatbox>();
        UIChatboxManager.instance.chatbox.PrintMessageOnChatBox($"{_chatbox.username}: {_chatbox.message}");
    }

    public static void RandomizeValidation(Packet _packet)
    {
        string stuff = "", color = "";
        int totalReady = _packet.ReadInt();
        int totalPlayer = _packet.ReadInt();

        if (totalReady == totalPlayer)
        {
            stuff = _packet.ReadString();
            color = _packet.ReadString();
        }

        UIRandomizeManager.instance.totalText.text = $"Total player're ready: {totalReady}/{totalPlayer}";
        UIRandomizeManager.instance.stuffText.text = $"You have to find some: {stuff}";
        UIRandomizeManager.instance.colorText.text = $"Your team's color: {color}";
    }
}
