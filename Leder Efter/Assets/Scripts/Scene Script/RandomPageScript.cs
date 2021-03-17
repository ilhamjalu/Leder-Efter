using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RandomPageScript : MonoBehaviour
{
    User.RandomObjective dataObj;
    User.RandomTeam dataTeam;

    [Header("General Attribute")]
    public NetworkScript network;
    public TMP_Text team, item;
    public Button randButton;

    private void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();
        randButton.onClick.AddListener(RequestRandom);
    }

    private void Randomizer()
    {
        IFormatter formatter = new BinaryFormatter();
        formatter.Binder = new CustomizedBinder();
        dataObj = (User.RandomObjective)formatter.Deserialize(network.ns);

        formatter = new BinaryFormatter();
        formatter.Binder = new CustomizedBinder();
        dataTeam = (User.RandomTeam)formatter.Deserialize(network.ns);

        team.text = dataTeam.team;
        item.text = "you have to find some " + dataObj.item;
    }

    public void RequestRandom()
    {
        network.writer.WriteLine("4");
        network.writer.Flush();

        Randomizer();
    }
}
