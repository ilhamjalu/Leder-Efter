using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Received packet via UDP. Contains message: {_msg}");
        ClientSend.UDPTestRequest();
    }

    public static void SignInValidation(Packet _packet)
    {
        string _msg = _packet.ReadString();
        UILoginManager.instance.notif.text = _msg;

        if (_msg == "login was successful")
            UILoginManager.instance.isLogin = true;
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

    public static void ReadyDong(Packet _packet)
    {
        string kondisi = "";
        int totalReady = _packet.ReadInt();
        int totalPlayer = _packet.ReadInt();

        if (totalReady == totalPlayer)
        {
            kondisi = _packet.ReadString();
        }

        UIRandomizeManager.instance.totalText.text = $"Total player're ready: {totalReady}/{totalPlayer}";
        UIRandomizeManager.instance.statusReady = kondisi;
    }

    public static void ColorHandler(Packet _packet)
    {
        var a = GameObject.Find("Soal");
        a.GetComponent<Text>().text = _packet.ReadString();
    }

    public static void PlayerPos(Packet _packet)
    {
        int id = _packet.ReadInt();
        string uname = _packet.ReadString();
        Vector3 pos = _packet.ReadVector3();
        Quaternion rot = _packet.ReadQuaternion();

        SpawnPlayer.sp.Spawn(id, uname, pos, rot);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 pos = _packet.ReadVector3();
        Debug.Log("UDP TEST");
        SpawnPlayer.players[id].transform.position = pos;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int id = _packet.ReadInt();
        Quaternion rot = _packet.ReadQuaternion();

        SpawnPlayer.players[id].transform.rotation = rot;
    }
}
