using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SceneMoving : MonoBehaviour
{
    public Button _this;
    public bool interactable;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        _this = this.GetComponent<Button>();
        _this.onClick.AddListener(CallingScene);
    }

    public void CallingScene()
    {
        Debug.Log(this.name);

        if (interactable && this.name == "Exit")
            Application.Quit();
        else if (interactable)
            SceneManager.LoadScene(sceneName);
    }
}
