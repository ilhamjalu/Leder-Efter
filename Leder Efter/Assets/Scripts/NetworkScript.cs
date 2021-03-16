﻿using System.Collections;
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

    [Header("Player Data")]
    public long identity;
    public string username;
    public string password;

    void Start()
    {
        client = new TcpClient(ipAddress, port);
        reader = new StreamReader(client.GetStream());
        writer = new StreamWriter(client.GetStream());

        DontDestroyOnLoad(this);
    }
}
