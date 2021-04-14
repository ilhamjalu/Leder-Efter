using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIRandomizeManager : MonoBehaviour
{
    public static UIRandomizeManager instance;
    public TextMeshProUGUI stuffText;
    public TextMeshProUGUI colorText;
    public TextMeshProUGUI totalText;

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

    public void GetReady()
    {
        ClientSend.RandomizeRequest();
    }
}
