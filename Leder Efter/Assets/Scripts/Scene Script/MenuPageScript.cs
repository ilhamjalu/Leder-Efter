using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuPageScript : MonoBehaviour
{
    [Header("General Attribute")]
    public NetworkScript network;
    public TextMeshProUGUI hello;

    void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();
        hello.text = "Hello, " + network.username;
    }

    void Update()
    {
        
    }
}
