using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIMenuManager : MonoBehaviour
{
    public static UIMenuManager instance;

    [Header("MainMenu Attribute")]
    public Client clientManager;
    public RoomDatabase roomManager;
    public TextMeshProUGUI helloText;
    public GameObject onlineChoice;
    public bool touched = false;

    [Header("HostJoin Attribute")]
    public InputField codeRoom;
    public TextMeshProUGUI notifText;
    public string goToScene;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }

    private void Start()
    {
        clientManager = GameObject.Find("ClientManager").GetComponent<Client>();
        roomManager = GameObject.Find("ClientManager").GetComponent<RoomDatabase>();
        helloText.text = $"hello, {clientManager.myUname} #{clientManager.myId}";
    }

    private void Update()
    {
        if (Client.instance.isPlay == 1)
            SceneManager.LoadScene(goToScene);
    }

    public void ChooseOnline(int code)
    {
        if (!touched && code == 1)
        {
            onlineChoice.SetActive(true);
            touched = true;
        }
        else if (touched)
        {
            onlineChoice.SetActive(false);
            touched = false;
        }
    }

    public void ChooseHost()
    {
        ClientSend.HostRoomRequest(codeRoom.text);
        roomManager.roomCode = codeRoom.text;
    }

    public void ChooseJoin()
    {
        ClientSend.JoinRoomRequest(codeRoom.text, clientManager.myUname);
        roomManager.roomCode = codeRoom.text;
    }
}
