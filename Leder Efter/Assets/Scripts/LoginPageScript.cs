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
    ClientData.SerializableData data;
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
        //Thread t = new Thread(Receive);
        //t.Start();

        signIn.onClick.AddListener(SignIn);
        signUp.onClick.AddListener(SignUp);
    }

    private void Update()
    {
        notifSignIn.text = recv;
        notifSignUp.text = recv;

        if (notifSignIn.text == "login was successful")
        {
            network.identity = Convert.ToInt32(network.reader.ReadLine());
            network.username = network.reader.ReadLine();
            network.password = network.reader.ReadLine();

            cooldown -= 1 * Time.deltaTime;
            if (cooldown <= 0)
                SceneManager.LoadScene(sceneLoad);
        }
    }

    public void Receive()
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
        else if (confirmPass.text == passSignUp.text)
        {
            Debug.Log("Masuk Sign In");
            network.writer.WriteLine("0");
            network.writer.Flush();

            network.ns = network.client.GetStream();
            network.formatter = new BinaryFormatter();
            network.formatter.Binder = new CustomizedBinder();
            data = new ClientData.SerializableData(userSignIn.text, passSignIn.text);
            network.formatter.Serialize(network.ns, data);

            recv = network.reader.ReadLine();
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
            network.writer.WriteLine("1");
            network.writer.Flush();

            network.ns = network.client.GetStream();
            network.formatter = new BinaryFormatter();
            network.formatter.Binder = new CustomizedBinder();
            data = new ClientData.SerializableData(userSignUp.text, passSignUp.text);
            network.formatter.Serialize(network.ns, data);
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

sealed class CustomizedBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type returntype = null;
        string sharedAssemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        assemblyName = Assembly.GetExecutingAssembly().FullName;
        typeName = typeName.Replace(sharedAssemblyName, assemblyName);
        returntype = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
        return returntype;
    }

    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        base.BindToName(serializedType, out assemblyName, out typeName);
        assemblyName = "SharedAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }
}
