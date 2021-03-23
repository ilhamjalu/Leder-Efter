using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LoginPageScript : MonoBehaviour
{
    [Header("General Attribute")]
    public NetworkScript network;
    public TMP_Text notif;
    public Thread receiveThread;
    public string sceneLoad;
    public string receive;
    public bool isLogin;

    [Header("SignIn Attribute")]
    public Button signIn;
    public TMP_InputField userSignIn, passSignIn;

    [Header("SignUp Attribute")]
    public Button signUp;
    public TMP_InputField userSignUp, passSignUp, confirmPass;

    private void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();

        signIn.onClick.AddListener(SignInSubmission);
        signUp.onClick.AddListener(SignUpSubmission);
    }

    private void Update()
    {
        notif.text = receive;
        if (isLogin)
            SceneManager.LoadScene(sceneLoad);
    }

    public void ReceiveNotification()
    {
        receive = network.reader.ReadLine();
        if (receive == "login was successful")
            ReceiveSignInData();
    }

    public void ReceiveSignInData()
    {
        IFormatter formatter_recv = new BinaryFormatter();
        formatter_recv.Binder = new CustomizedBinder();
        User.LoginData data_recv = (User.LoginData)formatter_recv.Deserialize(network.ns);

        network.identity = data_recv.identity;
        network.username = data_recv.username;
        network.password = data_recv.password;
        Debug.Log(data_recv.identity + " | " + data_recv.username + " | " + data_recv.password);

        network.indexMenu = 1;
        isLogin = true;
    }

    public void SignInSubmission()
    {
        Debug.Log("Sign In");
        if (userSignIn.text == "" || passSignIn.text == "")
            receive = "please fill in the blank field";
        else
        {
            network.writer.WriteLine("1");
            network.writer.Flush();

            IFormatter formatter_send = new BinaryFormatter();
            formatter_send.Binder = new CustomizedBinder();
            User.LoginData data_send = new User.LoginData(0, userSignIn.text, passSignIn.text);
            formatter_send.Serialize(network.ns, data_send);

            ValueReset();
        }
    }

    public void SignUpSubmission()
    {
        Debug.Log("Sign Up");
        if (userSignUp.text == "" || passSignUp.text == "" || confirmPass.text == "")
            receive = "please fill in the blank field";
        else if (passSignUp.text != confirmPass.text)
            receive = "please check the confirm password field";
        else
        {
            network.writer.WriteLine("2");
            network.writer.Flush();

            IFormatter formatter_send = new BinaryFormatter();
            formatter_send.Binder = new CustomizedBinder();
            User.LoginData data_send = new User.LoginData(0, userSignUp.text, passSignUp.text);
            formatter_send.Serialize(network.ns, data_send);

            ValueReset();
        }
    }

    public void ValueReset()
    {
        notif.text = "";
        userSignIn.text = "";
        userSignUp.text = "";
        passSignIn.text = "";
        passSignUp.text = "";
        confirmPass.text = "";
    }
}