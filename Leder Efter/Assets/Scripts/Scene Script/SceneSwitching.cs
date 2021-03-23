using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    [Header("General")]
    public NetworkScript network;

    [Header("Buttons")]
    public Button chatboxButton;
    public Button randomizeButton;

    void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();

        chatboxButton.onClick.AddListener(ChooseChatbox);
        randomizeButton.onClick.AddListener(ChooseRandomize);
    }

    public void ChooseChatbox()
    {
        network.indexMenu = 1;
        SceneManager.LoadScene("Chatbox");
    }
    
    public void ChooseRandomize()
    {
        network.indexMenu = 2;
        SceneManager.LoadScene("Randomize");
    }
}
