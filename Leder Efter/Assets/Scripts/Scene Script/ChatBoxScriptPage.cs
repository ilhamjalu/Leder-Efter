using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChatBoxScriptPage : MonoBehaviour
{
    [Header("General Attribute")]
    public NetworkScript network;
    public Button sendButton;
    public TMP_InputField inputText;
    public TextMeshProUGUI helloText;
    public TextMeshProUGUI informationText;
    public TextMeshProUGUI historyChatText;
    public string recv;

    void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();
        Thread receiver = new Thread(ReceiveMessage);
        receiver.Start();

        sendButton.onClick.AddListener(SubmitMessage);
    }

    void Update()
    {
        historyChatText.text = recv;
        helloText.text = "Hello, " + network.username;
    }

    void SubmitMessage()
    {
        if (inputText.text == "")
            informationText.text = "please input the message in the blank field";
        else
        {
            network.writer.WriteLine("3");
            network.writer.Flush();

            network.writer.WriteLine(inputText.text);
            network.writer.Flush();

            informationText.text = "your message sent";
            inputText.text = "";
        }
    }

    void ReceiveMessage()
    {
        while (true)
        {
            recv = network.reader.ReadLine();
        }
    }
}
