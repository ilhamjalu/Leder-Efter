using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LoginTransition : MonoBehaviour
{
    [Header("Main Menu Attribute")]
    public LoginPageScript login;
    public Button toSignIn, toSignUp;
    public GameObject leftPosition, centerPosition, rightPosition;
    public GameObject signInPanel, signUpPanel;
    public float speed;
    public int menu;

    void Start()
    {
        login = GameObject.Find("BackgroundScript").GetComponent<LoginPageScript>();
        toSignIn.onClick.AddListener(ChangeToSignIn);
        toSignUp.onClick.AddListener(ChangeToSignUp);
    }

    void Update()
    {
        switch (menu)
        {
            case 0:
                MovingPanel(signInPanel, centerPosition);
                MovingPanel(signUpPanel, rightPosition);
                break;
            case 1:
                MovingPanel(signUpPanel, centerPosition);
                MovingPanel(signInPanel, leftPosition);
                break;
        }
    }

    public void MovingPanel(GameObject panel, GameObject target)
    {
        panel.transform.position = Vector3.MoveTowards(panel.transform.position, 
                                                       target.transform.position, 
                                                       speed);
    }

    public void ChangeToSignIn()
    {
        login.recv = null;
        menu = 0;
    }
    public void ChangeToSignUp()
    {
        login.recv = null;
        menu = 1;
    }
}
