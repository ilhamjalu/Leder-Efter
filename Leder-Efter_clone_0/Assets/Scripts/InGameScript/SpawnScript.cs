using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Warna
{
    public string Word;
    public Color Value;

    public Warna(string _word, Color _value)
    {
        Word = _word;
        Value = _value;
    }
}

public class SpawnScript : MonoBehaviour
{
    public Text soal;
    public GameObject[] spawn;
    public List<Warna> warna = new List<Warna>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        warna.Add(new Warna("Merah", Color.red));
        warna.Add(new Warna("Biru", Color.blue));
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RandomPick();
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        for (int i = 0; i < warna.Count; i++)
        {
            if (warna[i].Word == soal.text)
            {
                for(int j = 0; j < spawn.Length; j++)
                {
                    if(spawn[j].gameObject.name == "1")
                    {
                        spawn[j].gameObject.GetComponent<Image>().color = warna[i].Value;
                        return;
                    }
                }
            }
            else
            {

            }
        }
    }

    public void RandomPick()
    {
        System.Random rand = new System.Random();
        int random = rand.Next(1,3);

        Debug.Log(random);
        spawn[0].gameObject.name = random.ToString();

        if (random == 1)
        {
            spawn[1].gameObject.name = "2";
        }
        else
        {
            spawn[1].gameObject.name = "1";
        }
    }
}
