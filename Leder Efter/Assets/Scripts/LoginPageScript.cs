//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using UnityEngine;
//using TMPro;
//using System;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Xml.Serialization;
//using System.Reflection;
//using System.Threading;

//public class LoginPageScript : MonoBehaviour
//{
//    ClientData.SerializableData data;
//    string recv;
//    //NetworkScript net;
//    //public NetworkStream ns;
//    //public IFormatter formatter;

//    [Header("General Attribute")]
//    public TMP_InputField username, password;
//    public TMP_Text notif;
//    public Button signIn;

//    [Header("Online Attribute")]
//    public TcpClient client;
//    public StreamReader reader;
//    public StreamWriter writer;
//    public NetworkStream ns;
//    public string ipAddress;
//    public IFormatter formatter;

//    private void Start()
//    {
//        client = new TcpClient(ipAddress, 8080);
//        reader = new StreamReader(client.GetStream());
//        writer = new StreamWriter(client.GetStream());
//        Thread t = new Thread(Receive);
//        t.Start();
//    }

//    private void Update()
//    {
//        notif.text = recv;
//    }

//    public void Receive()
//    {
//        recv = reader.ReadLine();
//    }

//    public void Login()
//    {
//        ns = client.GetStream();
//        formatter = new BinaryFormatter();
//        formatter.Binder = new CustomizedBinder();
//        data = new ClientData.SerializableData(username.text, password.text);
//        formatter.Serialize(ns, data);
//        Debug.Log("TEST");
//        //Debug.Log(formatter);
//    }
//}

//sealed class CustomizedBinder : SerializationBinder
//{
//    public override Type BindToType(string assemblyName, string typeName)
//    {
//        Type returntype = null;
//        string sharedAssemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
//        assemblyName = Assembly.GetExecutingAssembly().FullName;
//        typeName = typeName.Replace(sharedAssemblyName, assemblyName);
//        returntype =
//                Type.GetType(String.Format("{0}, {1}",
//                typeName, assemblyName));

//        return returntype;
//    }

//    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
//    {
//        base.BindToName(serializedType, out assemblyName, out typeName);
//        assemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
//    }
//}
