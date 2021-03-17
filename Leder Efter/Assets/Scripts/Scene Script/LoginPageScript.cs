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
    User.LoginData data;
    float cooldown;

    [Header("General Attribute")]
    public NetworkScript network;
    public LoginTransition menu;
    public string sceneLoad;
    public string recv;

    [Header("SignIn Attribute")]
    public TMP_Text notifSignIn;
    public TMP_InputField userSignIn, passSignIn;
    public Button signIn;

    [Header("SignUp Attribute")]
    public TMP_Text notifSignUp;
    public TMP_InputField userSignUp, passSignUp, confirmPass;
    public Button signUp;

    private void Start()
    {
        network = GameObject.Find("NetworkScript").GetComponent<NetworkScript>();
        menu = GameObject.Find("BackgroundScript").GetComponent<LoginTransition>();
        //Thread t = new Thread(ReceiveDatabase);
        //t.Start();

        signIn.onClick.AddListener(SignIn);
        signUp.onClick.AddListener(SignUp);
    }

    private void Update()
    {
        notifSignIn.text = recv;
        notifSignUp.text = recv;

        if (network.username != "" || network.password != "")
            SceneManager.LoadScene(sceneLoad);
    }

    public void ReceiveDatabase()
    {
        while (true)
        {
            recv = network.reader.ReadLine();
        }
    }

    public void SignIn()
    {
        if (userSignIn.text == "" || passSignIn.text == "")
            recv = "fill in the blank field!";
        else
        {
            Debug.Log("Masuk Sign In");
            network.writer.WriteLine("1");
            network.writer.Flush();

            IFormatter formatter_send = new BinaryFormatter();
            formatter_send.Binder = new CustomizedBinder();
            User.LoginData data_send = new User.LoginData(userSignIn.text, passSignIn.text);
            formatter_send.Serialize(network.ns, data_send);

            recv = network.reader.ReadLine();
            if (recv == "login was successful")
            {
                IFormatter formatter_recv = new BinaryFormatter();
                formatter_recv.Binder = new CustomizedBinder();
                User.LoginData data_recv = (User.LoginData)formatter_recv.Deserialize(network.ns);

                network.username = data_recv.name;
                network.password = data_recv.pass;
            }
        }

        ValueReset();
    }
    public void SignUp()
    {
        if (userSignUp.text == "" || confirmPass.text == "" || passSignUp.text == "")
            recv = "fill in the blank field!";
        else if (confirmPass.text != passSignUp.text)
            recv = "password are not matching!";
        else if (confirmPass.text == passSignUp.text)
        {
            Debug.Log("Masuk Sign Up");
            network.writer.WriteLine("2");
            network.writer.Flush();

            IFormatter formatter_send = new BinaryFormatter();
            formatter_send.Binder = new CustomizedBinder();
            User.LoginData data_send = new User.LoginData(userSignUp.text, passSignUp.text);
            formatter_send.Serialize(network.ns, data_send);

            recv = network.reader.ReadLine();
            if (recv == "account successfully registered")
                menu.menu = 0;
        }

        ValueReset();
    }

    public void ValueReset()
    {
        notifSignIn.text = null;
        notifSignUp.text = null;
        userSignIn.text = null;
        userSignUp.text = null;
        passSignIn.text = null;
        passSignUp.text = null;
        confirmPass.text = null;
    }
}