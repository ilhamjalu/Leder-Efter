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
    public int playerScore;
    public TextMeshProUGUI helloText;
    public TextMeshProUGUI scoreText;
    public GameObject onlineChoice;
    public GameObject quitChoice;
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
        helloText.text = $"hello, {Client.instance.myUname} #{Client.instance.myId}";
        ClientSend.ScoreRequest(Client.instance.myUname);
    }

    private void Update()
    {
        if (Client.instance.isPlay)
            SceneManager.LoadScene(goToScene);

        scoreText.text = "Score : " + playerScore;
    }

    public void GameTest()
    {
        SceneManager.LoadScene("PropHunt");
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
        if (codeRoom.text != "")
        {
            ClientSend.HostRoomRequest(codeRoom.text);
            RoomDatabase.instance.roomCode = codeRoom.text;
        }
        else
        {
            notifText.text = "please input the room code!";
        }
    }

    public void ChooseJoin()
    {
        if (codeRoom.text != "")
        {
            ClientSend.JoinRoomRequest(codeRoom.text, Client.instance.myId, Client.instance.myUname);
            RoomDatabase.instance.roomCode = codeRoom.text;
        }
        else
        {
            notifText.text = "please input the room code!";
        }
    }

    public void ChooseQuit(int index)
    {
        if (index == 0)
            quitChoice.SetActive(true);
        else if (index == 1)
            quitChoice.SetActive(false);
        else if (index == 2)
        {
            Client.instance.Disconnect();
            SceneManager.LoadScene("LoginPage");
        }
    }
}
