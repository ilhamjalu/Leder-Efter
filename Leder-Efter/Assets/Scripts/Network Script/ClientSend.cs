﻿using System.Collections;
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

    public static void HostRoomRequest(string code)
    {
        using (Packet _packet = new Packet((int)ClientPackets.hostRoomRequest))
        {
            _packet.Write(code);
            SendTCPData(_packet);
        }
    }

    public static void JoinRoomRequest(string code, int id, string uname)
    {
        using (Packet _packet = new Packet((int)ClientPackets.joinRoomRequest))
        {
            _packet.Write(code);
            _packet.Write(id);
            _packet.Write(uname);
            SendTCPData(_packet);
        }
    }

    public static void LeaveRoomRequest(string code, string uname)
    {
        using (Packet _packet = new Packet((int)ClientPackets.leaveRoomRequest))
        {
            _packet.Write(code);
            _packet.Write(uname);
            SendTCPData(_packet);
        }
    }

    public static void DestroyRoomRequest(string code)
    {
        using (Packet _packet = new Packet((int)ClientPackets.destroyRoomRequest))
        {
            _packet.Write(code);
            SendTCPData(_packet);
        }
    }

    public static void StartMatch(string code, string gameType)
    {
        using (Packet _packet = new Packet((int)ClientPackets.startMatchRequest))
        {
            _packet.Write(code);
            _packet.Write(gameType);
            SendTCPData(_packet);
        }
    }

    public static void PlayerPosition(string code, int id, int controlHorizontal, int controlVertical)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerPositionRequest))
        {
            _packet.Write(code);
            _packet.Write(id);
            _packet.Write(controlHorizontal);
            _packet.Write(controlVertical);
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

    public static void Gaskeun()
    {
        using (Packet _packet = new Packet((int)ClientPackets.readyGan))
        {
            _packet.Write(true);
            SendTCPData(_packet);
        }
    }

    public static void ColorRequest()
    {
        using (Packet _packet = new Packet((int)ClientPackets.colorRequest))
        {
            _packet.Write(true);
            SendTCPData(_packet);
        }
    }

    public static void Mintak()
    {
        using (Packet _packet = new Packet((int)ClientPackets.mintakSpawnDong))
        {   
            SendTCPData(_packet);
        }
    }

    public static void TriviaRequest()
    {
        using (Packet _packet = new Packet((int)ClientPackets.triviaQuestionRequest))
        {
            _packet.Write(true);
            _packet.Write(UITriviaManager.instance.trivia.Count);
            SendTCPData(_packet);
        }
    }

    public static void SendPos(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach(bool input in _inputs)
            {
                _packet.Write(input);
            }
            _packet.Write(SpawnPlayer.players[Client.instance.myId].transform.rotation);
            SendUDPData(_packet);
        }
    }

    public static void UpScore(string uname, int score)
    {
        using (Packet _packet = new Packet((int)ClientPackets.storeScore))
        {
            _packet.Write(uname);
            _packet.Write(score);
            SendTCPData(_packet);
        }
    }

    public static void ScoreRequest(string uname)
    {
        using (Packet _packet = new Packet((int)ClientPackets.scoreRequest))
        {
            _packet.Write(uname);
            SendTCPData(_packet);
        }
    }
    #endregion
}