using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.Reflection;
using System.Threading;

public class Random : MonoBehaviour
{
    ClientData.Player data;
    public TMP_Text warna, item;
    public NetworkScript network;
    public string test;

    private void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();
        //Thread t = new Thread(ReceiveTest);
        //t.Start();
    }

    private void Update()
    {

    }

    //void ReceiveTest()
    //{
    //    if (test == "")
    //    {
    //        test = network.reader.ReadLine();
    //        Debug.Log("TEST");
    //    }
    //    else
    //    {
    //        Debug.Log("JANCOK");
    //    }
    //}

    private void Randomizer()
    {
        if (test == "ok")
        {
            ClientData.Player data;
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new CustomizedBinder();
            data = (ClientData.Player)formatter.Deserialize(network.ns);

            warna.text = data.team;
            item.text = data.item;

            Debug.Log(data.item);
        }
        else
        {
            return;
        }
    }

    public void Acak()
    {
        network.writer.WriteLine("4");
        network.writer.Flush();
        test = network.reader.ReadLine();

        Randomizer();
    }
}
