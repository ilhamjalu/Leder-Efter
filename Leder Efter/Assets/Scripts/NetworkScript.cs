using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class NetworkScript : MonoBehaviour
{
    [Header("Online Attribute")]
    public TcpClient client;
    public StreamReader reader;
    public StreamWriter writer;
    public string ipAddress;

    void Start()
    {
        client = new TcpClient(ipAddress, 8080);
        reader = new StreamReader(client.GetStream());
        writer = new StreamWriter(client.GetStream());

        DontDestroyOnLoad(this);
    }
}
