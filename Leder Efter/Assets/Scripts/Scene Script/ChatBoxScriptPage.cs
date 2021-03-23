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
    public ChatBoxSystem chatbox;
    public NetworkScript network;
    public TMP_InputField inputText;
    public Button sendButton;
    public Thread receiveThread;
    public string recv;

    void Start()
    {
        chatbox = this.gameObject.GetComponent<ChatBoxSystem>();
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();

        sendButton.onClick.AddListener(SubmitMessage);
    }

    void Update()
    {
        if (recv != "")
        {
            chatbox.PrintMessageOnChatBox(recv);
            recv = "";
        }
    }

    void SubmitMessage()
    {
        network.writer.WriteLine("3");
        network.writer.Flush();

        network.writer.WriteLine(inputText.text);
        network.writer.Flush();

        inputText.text = "";
    }

    public void ReceiveMessage()
    {
        recv = network.reader.ReadLine();
    }
}
