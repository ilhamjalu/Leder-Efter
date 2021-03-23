using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System;

public class FPSPageScript : MonoBehaviour
{
    [Header("Buttons")]
    public Button shootButton;
    public Button crouchButton;
    public Button runButton;

    [Header("General")]
    public NetworkScript network;
    public ChatBoxSystem box;
    public string history;
    public int healthPoint;
    public bool recvDone;

    [Header("Home")]
    public TextMeshProUGUI nameHomeText;
    public TextMeshProUGUI healthHomeText;
    public string nameHome;
    public int healthHome;

    [Header("Away")]
    public TextMeshProUGUI nameAwayText;
    public TextMeshProUGUI healthAwayText;
    public string nameAway;
    public int healthAway;

    void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();

        shootButton.onClick.AddListener(ShootAction);
        crouchButton.onClick.AddListener(CrouchAction);
        runButton.onClick.AddListener(RunAction);
    }

    void Update()
    {
        if (recvDone)
        {
            nameHomeText.text = nameHome;
            nameAwayText.text = nameAway;

            healthHomeText.text = "HP: " + healthHome.ToString();
            healthAwayText.text = "HP: " + healthAway.ToString();

            box.PrintMessageOnChatBox(history);
        }
    }

    public void SubmitData(int action)
    {
        network.writer.WriteLine("5");
        network.writer.Flush();

        switch (action)
        {
            case 1:
                IFormatter formatter_shoot = new BinaryFormatter();
                formatter_shoot.Binder = new CustomizedBinder();
                User.FPSData data_shoot = new User.FPSData(network.username, 0, 0, true, false, false);
                formatter_shoot.Serialize(network.ns, data_shoot);
                break;

            case 2:
                IFormatter formatter_crouch = new BinaryFormatter();
                formatter_crouch.Binder = new CustomizedBinder();
                User.FPSData data_crouch = new User.FPSData(network.username, 0, 0, false, true, false);
                formatter_crouch.Serialize(network.ns, data_crouch);
                break;

            case 3:
                IFormatter formatter_run = new BinaryFormatter();
                formatter_run.Binder = new CustomizedBinder();
                User.FPSData data_run = new User.FPSData(network.username, 0, 0, false, false, true);
                formatter_run.Serialize(network.ns, data_run);
                break;
        }
    }

    public void ReceiveData()
    {
        int range = Convert.ToInt32(network.reader.ReadLine());
        for (int i = 0; i < range; i++)
        {
            IFormatter formatter_recv = new BinaryFormatter();
            formatter_recv.Binder = new CustomizedBinder();
            User.FPSData data_recv = (User.FPSData)formatter_recv.Deserialize(network.ns);
            history = data_recv.username + " w/ HP: " +
                            data_recv.healthPoint + " shoot: " +
                            data_recv.shootAccuracy + "%, crouch: " +
                            data_recv.crouch + " run: " +
                            data_recv.run;

            if (data_recv.username == network.username)
            {
                nameHome = data_recv.username;
                healthHome = data_recv.healthPoint;
            }
            else if (data_recv.username != network.username)
            {
                nameAway = data_recv.username;
                healthAway = data_recv.healthPoint;
            }

            Debug.Log(history);
            recvDone = true;
        }
    }


    public void ShootAction()
    {
        SubmitData(1);
    }
    public void CrouchAction()
    {
        SubmitData(2);
    }
    public void RunAction()
    {
        SubmitData(3);
    }
}
