using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button chatboxButton;
    public Button randomizerButton;
    public Button fpsButton;

    [Header("Scene Target")]
    public List<GameObject> sceneTarget;

    private void Start()
    {
        chatboxButton.onClick.AddListener(ToScenechatboxButton);
        randomizerButton.onClick.AddListener(ToScenerandomizerButton);
        fpsButton.onClick.AddListener(ToScenefpsButton);

        chatboxButton.interactable = false;
        for (int i = 1; i < sceneTarget.Count; i++)
        {
            sceneTarget[i].SetActive(false);
        }
    }

    private void Update()
    {
        chatboxButton = GameObject.Find("ChatboxButton").GetComponent<Button>();
        randomizerButton = GameObject.Find("RandomizeButton").GetComponent<Button>();
        fpsButton = GameObject.Find("FPSButton").GetComponent<Button>();
    }

    public void ToScenechatboxButton()
    {
        chatboxButton.interactable = false;
        randomizerButton.interactable = true;
        fpsButton.interactable = true;

        SettingScene(0);
    }

    public void ToScenerandomizerButton()
    {
        chatboxButton.interactable = true;
        randomizerButton.interactable = false;
        fpsButton.interactable = true;

        SettingScene(1);
    }

    public void ToScenefpsButton()
    {
        chatboxButton.interactable = true;
        randomizerButton.interactable = true;
        fpsButton.interactable = false;

        SettingScene(2);
    }

    public void SettingScene(int thisScene)
    {
        for (int i = 0; i < sceneTarget.Count; i++)
        {
            sceneTarget[i].SetActive(false);
            
            if (i == thisScene)
                sceneTarget[i].SetActive(true);
        }
    }
}
