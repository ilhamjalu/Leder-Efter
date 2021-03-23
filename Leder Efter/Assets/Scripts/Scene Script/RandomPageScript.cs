using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RandomPageScript : MonoBehaviour
{
    [Header("General Attribute")]
    public NetworkScript network;
    public TMP_Text teamText, itemText;
    public Button requestRand;
    public Thread receiveThread;
    public string team, item;

    private void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();
        requestRand.onClick.AddListener(RequestRandom);
    }

    void Update()
    {
        teamText.text = "Your Team's Color: " + team;
        itemText.text = "You Have To Find Some: " + item;
    }

    public void RequestRandom()
    {
        network.writer.WriteLine("4");
        network.writer.Flush();
    }

    public void RandomReceiver()
    {
        IFormatter formatter_recv = new BinaryFormatter();
        formatter_recv.Binder = new CustomizedBinder();
        User.RandomTeam data_team = (User.RandomTeam)formatter_recv.Deserialize(network.ns);

        IFormatter formatter_recv2 = new BinaryFormatter();
        formatter_recv2.Binder = new CustomizedBinder();
        User.RandomObjective data_item = (User.RandomObjective)formatter_recv.Deserialize(network.ns);

        team = data_team.team;
        item = data_item.item;
    }
}
