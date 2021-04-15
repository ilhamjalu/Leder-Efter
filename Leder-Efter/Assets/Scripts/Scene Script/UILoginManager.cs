using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UILoginManager : MonoBehaviour
{
    public static UILoginManager instance;

    public GameObject startMenu;
    public GameObject loginPage;
    public InputField ipField;
    public TextMeshProUGUI notif;
    public string toScene;

    [Header("SignIn Attribute")]
    public TMP_InputField usernameSignIn;
    public TMP_InputField passwordSignIn;

    [Header("SignUp Attribute")]
    public TMP_InputField usernameSignUp;
    public TMP_InputField passwordSignUp;
    public TMP_InputField confirmSignUp;

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

    private void Start()
    {
        loginPage.SetActive(false);
    }

    private void Update()
    {
        if (Client.instance.isLogin == 1)
        {
            SceneManager.LoadScene(toScene);
        }

        //if (PlayerPrefs.GetInt("ClientIsLogin") == 1)
        //{
        //    ClientSend.SignInRequest(PlayerPrefs.GetString("ClientUname"), PlayerPrefs.GetString("ClientPass"));
        //}
    }

    public void ConnectToServer()
    {
        ipField.interactable = false;

        if (ipField.text == "")
            ipField.text = "127.0.0.1";

        Client.instance.ip = ipField.text;
        Client.instance.ConnectToServer();

        startMenu.SetActive(false);
        loginPage.SetActive(true);
    } 

    public void SignIn()
    {
        if (usernameSignIn.text == "" || passwordSignIn.text == "")
            notif.text = "please fill the blank field!";
        else
        {
            Client.instance.myUname = usernameSignIn.text;
            Client.instance.myPass = passwordSignIn.text;
            ClientSend.SignInRequest(usernameSignIn.text, passwordSignIn.text);
            ValueReset();
        }
    }

    public void SignUp()
    {
        if (usernameSignUp.text == "" || passwordSignUp.text == "" || confirmSignUp.text == "")
            notif.text = "please fill the blank field!";
        else if (passwordSignUp.text != confirmSignUp.text)
            notif.text = "please re-check the confirm password field!";
        else
        {
            ClientSend.SignUpRequest(usernameSignUp.text, passwordSignUp.text);
            ValueReset();
        }
    }

    public void ValueReset()
    {
        usernameSignIn.text = "";
        passwordSignIn.text = "";
        usernameSignUp.text = "";
        passwordSignUp.text = "";
        confirmSignUp.text = "";
    }
}
