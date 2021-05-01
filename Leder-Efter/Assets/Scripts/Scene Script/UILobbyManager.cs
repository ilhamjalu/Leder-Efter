using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerPrint 
{
    public GameObject background;
    public Text playerName;

    public PlayerPrint(GameObject gameobject, Text text)
    {
        background = gameobject;
        playerName = text;
    }
}

public class UILobbyManager : MonoBehaviour
{
    public static UILobbyManager instance;

    [Header("Lobby Attribute")]
    public TextMeshProUGUI helloText;
    public RoomDatabase roomManager;
    public GameObject readyButton;

    [Header("Player Data Attribute")]
    public List<PlayerPrint> playerPrint;

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

    void Start()
    {
        roomManager = GameObject.Find("ClientManager").GetComponent<RoomDatabase>();
        helloText.text = $"you're in room #{roomManager.roomCode}";

        if (Client.instance.host)
            readyButton.SetActive(true);
    }

    void Update()
    {
        for (int i = 0; i < roomManager.playerDatabase.Count; i++)
        {
            playerPrint[i].background.SetActive(true);
            playerPrint[i].playerName.text = roomManager.playerDatabase[i].username;
        }
    }
}
