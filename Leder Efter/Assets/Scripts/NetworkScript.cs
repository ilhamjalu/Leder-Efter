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

sealed class CustomizedBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type returntype = null;
        string sharedAssemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        assemblyName = Assembly.GetExecutingAssembly().FullName;
        typeName = typeName.Replace(sharedAssemblyName, assemblyName);
        returntype = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
        return returntype;
    }

    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        base.BindToName(serializedType, out assemblyName, out typeName);
        assemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }
}