using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;

public class NetworkScript : MonoBehaviour
{
    [Header("Online Attribute")]
    public TcpClient client;
    public StreamReader reader;
    public StreamWriter writer;
    public NetworkStream ns;
    public IFormatter formatter;
    public string ipAddress;
    public int port;

    [Header("Player Attribute")]
    public int identity;
    public string username;
    public string password;

    [Header("General")]
    public LoginPageScript login;
    public ChatBoxScriptPage chatbox;
    public RandomPageScript randomize;
    public FPSPageScript fps;
    public int indexMenu;

    void Awake()
    {
        client = new TcpClient(ipAddress, port);
        ns = client.GetStream();
        reader = new StreamReader(ns);
        writer = new StreamWriter(ns);

        Thread ThreadHandler = new Thread(ClientReceiver);
        ThreadHandler.Start();

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (login == null && indexMenu == 0)
            login = GameObject.Find("Login Panel").GetComponent<LoginPageScript>();
        else if (chatbox == null && indexMenu == 1)
            chatbox = GameObject.Find("Chatbox Panel").GetComponent<ChatBoxScriptPage>();
        else if (randomize == null && indexMenu == 2)
            randomize = GameObject.Find("Randomize Panel").GetComponent<RandomPageScript>();
        else if (fps == null && indexMenu == 3)
            fps = GameObject.Find("FPS Panel").GetComponent<FPSPageScript>();
    }

    public void ClientReceiver()
    {
        while (true)
        {
            string process = reader.ReadLine();
            switch (process)
            {
                case "1":
                    login.ReceiveNotification();
                    break;

                case "2":
                    login.ReceiveNotification();
                    break;

                case "3":
                    chatbox.ReceiveMessage();
                    break;

                case "4":
                    randomize.RandomReceiver();
                    break;

                case "5":
                    fps.ReceiveData();
                    break;
            }

            process = "";
        }
    }

    public void DisconnectFromServer()
    {
        client.Close();
        ns.Close();
        reader.Close();
        writer.Close();
    }
}
