using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnaSoal
{
    public string Word;
    public Color Value;

    public WarnaSoal(string _word, Color _value)
    {
        Word = _word;
        Value = _value;
    }
}

public class ColorManager : MonoBehaviour
{
    public List<Warna> warnaSoal = new List<Warna>();
    public bool checkWarna = false;

    // Start is called before the first frame update
    void Start()
    {
        ClientSend.ColorRequest();
        warnaSoal.Add(new Warna("Merah", Color.red));
        warnaSoal.Add(new Warna("Biru", Color.blue));
        warnaSoal.Add(new Warna("Hijau", Color.green));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    ClientSend.ColorRequest();
        //}
        if (checkWarna == true)
        {
            //ClientSend.ColorRequest();
            Debug.Log("TEST DEBUG");
        }
    }
}
