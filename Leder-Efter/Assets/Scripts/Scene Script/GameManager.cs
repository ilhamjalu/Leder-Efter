using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> playerCharacterTemp;
    public List<GameObject> player;

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
        for (int i = 0; i < RoomDatabase.instance.playerDatabase.Count; i++)
        {
            var player = Instantiate(playerCharacterTemp[RoomDatabase.instance.playerDatabase[i].typeChar],
                                     new Vector3(0, 0, 0),
                                     Quaternion.identity);

            player.transform.parent = gameObject.transform;
            player.name = RoomDatabase.instance.playerDatabase[i].username;
            RoomDatabase.instance.playerDatabase[i].character = player.GetComponent<PlayerCharManager>();
        }
    }

    void Update()
    {
        MoveRequest();
    }

    public void MoveRequest()
    {
        int horizontal = Convert.ToInt32(Input.GetAxis("Horizontal"));
        int vertical = Convert.ToInt32(Input.GetAxis("Vertical"));

        ClientSend.PlayerPosition(RoomDatabase.instance.roomCode,
                                  Client.instance.myId, horizontal, vertical);
    }

    public void UpdatePlayerPosition(int id, int _controlHorizontal, int _controlVertical)
    {
        for (int i = 0; i < RoomDatabase.instance.playerDatabase.Count; i++)
        {
            if (RoomDatabase.instance.playerDatabase[i].id == id)
            {
                RoomDatabase.instance.playerDatabase[i].character.horizontal = _controlHorizontal;
                RoomDatabase.instance.playerDatabase[i].character.vertical = _controlVertical;
            }
        }
    }
}
