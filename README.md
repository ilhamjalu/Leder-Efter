### Leder-Efter
(Author) 4210181002 Farhan Muhammad
(Author) 4210181010 Ilham Jalu Prakosa
(Course) Praktikum Desain Multiplayer Game Online

### Game-Overview
This game can be played by up to 30 players. Every five players will enter into a team with the same color, each team will look for items that are distributed to several random positions. Win / lose conditions will be determined from the total items found by each team.

### Game-Detail
Multiplayer Game Online
Platform: Windows
Protocol: TCP
Game-Engine: Unity 2019 LTS

### High-Concept-Document
High Concept Document can be accessed in here

### Game-Design-Document
Game Concept Document can be accessed in here

### GIF-Progress
4210181002_4210181010_FPS Gameplay

### Game Documentation

#### Packets
packets is a collection of some data that going to send from server to clients or send from clients to server. these packet have an identifier to make server or clients know what should they do when they got that packet, below is the packets identifier that located on server and clients, server and client must have 2 of these packets
```C#
public enum ServerPackets
    {
        welcome = 1,
        signIn,
        signUp,
        hostRoom,
        joinRoom,
        leaveRoom,
        destroyRoom,
        playerJoined,
        startMatch,
        playerPosition,

        chatbox,
        triviaQuestion,
        triviaDatabase,
        scorePlaySent
    }
```
```C#
public enum ClientPackets
{
    welcomeRequest = 1,
    signInRequest,
    signUpRequest,
    hostRoomRequest,
    joinRoomRequest,
    leaveRoomRequest,
    destroyRoomRequest,
    startMatchRequest,
    playerPositionRequest,

    chatboxRequest,
    triviaQuestionRequest,
    triviaDatabaseRequest,
    storeScorePlay,
    scorePlayRequest
}
```
#### Sign In
on this login page client must sign in before can play the game, but when user doesnt have the account, they can use sign up feature
Login Image
if user already have an account to sign in, on clients side function SignInRequest will be running, this function use to send data such as username and password to server, this packets have an identifier that called signInRequest
```C#
    public static void SignInRequest(string uname, string pass)
    {
        using (Packet _packet = new Packet((int)ClientPackets.signInRequest))
        {
            ClientData.Account _account = new ClientData.Account(uname, pass);

            _packet.Write<ClientData.Account>(_account);
            SendTCPData(_packet);
        }
    }
```
on the signInRequest function they called a clientData.Account that contain account data, below is code of Account
```C#
namespace ClientData
{
    [System.Serializable]
    public class Account
    {
        public string username;
        public string password;

        public Account(string uname, string pass)
        {
            username = uname;
            password = pass;
        }
    }
}
```
clientData.Account is a way to call Account class, then after this class filled by user input on the sign in page, the packet is ready to send to the server, on the server the must run a function corresponding to the packets identifier that client send, below is the code:
```C#
packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeRequest, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.signInRequest, ServerHandle.SignInReceived },
                { (int)ClientPackets.signUpRequest, ServerHandle.SignUpReceived },
                { (int)ClientPackets.hostRoomRequest, ServerHandle.HostRoomReceived },
                { (int)ClientPackets.joinRoomRequest, ServerHandle.JoinRoomReceived },
                { (int)ClientPackets.leaveRoomRequest, ServerHandle.LeaveRoomReceived },
                { (int)ClientPackets.destroyRoomRequest, ServerHandle.DestroyRoomReceived },
                { (int)ClientPackets.startMatchRequest, ServerHandle.StartMatchReceived },
                { (int)ClientPackets.playerPositionRequest, ServerHandle.PlayerPositionReceived },

                { (int)ClientPackets.chatboxRequest, ServerHandle.ChatboxReceived },
                { (int)ClientPackets.randomizeRequest, ServerHandle.RandomizeReceived },
                { (int)ClientPackets.colorRequest, ServerHandle.ColorReceived },
                { (int)ClientPackets.mintakSpawnDong, ServerHandle.MintakPlayer },
                { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
                { (int)ClientPackets.readyGan, ServerHandle.PlayerReady },

                { (int)ClientPackets.triviaQuestionRequest, ServerHandle.TriviaQuestionReceived },
                { (int)ClientPackets.storeScore, ServerHandle.ScoreReceived },
                { (int)ClientPackets.scoreRequest, ServerHandle.ScoreSent },
                { (int)ClientPackets.storePlay, ServerHandle.PlayReceived },
                { (int)ClientPackets.playRequest, ServerHandle.PlaySent }
            };
```

so from the code above server must run SignInReceived Class to check that the user input is true or false, in this class another class is call to do a validation from user input
```C#
public static void SignInReceived(int _fromClient, Packet _packet)
        {
            ClientData.Account _account = _packet.ReadObject<ClientData.Account>();

            string validation = AccountHandler.SignIn(_account.username, _account.password);
            if (validation == "login was successful")
                Server.readyDatabase.Add(new ReadyDatabase(_fromClient, false));

            ServerSend.SignInValidation(_fromClient, validation);
        }
```

AccountHandler is a class to do all of something that have relation with user Account, and from the code above, Account Handler will do a validation from user input
```c#
public static string SignIn(string uname, string pass)
        {
            Server.accountDatabase = LoadDatabase<List<AccountDatabase>>("AccountDatabase.xml");

            foreach (AccountDatabase oacc in Server.accountDatabase)
            {
                if (uname == oacc.username && !oacc.active)
                {
                    if (pass == oacc.password)
                    {
                        oacc.active = true;
                        Console.WriteLine($"There's player signIn: {uname}");

                        oacc.identity = Client.identity;
                        SaveDatabase(Server.accountDatabase, "AccountDatabase.xml");
                        return "login was successful";
                    }
                    else
                    {
                        return "login failed! your password is wrong";
                    }
                }
                else if (uname == oacc.username && oacc.active)
                {
                    return "login failed! another user is using your account";
                }
            }

            return "login failed! your account's not found";
        }
```

to check account, server will call a function SaveDatabase that will do a serialize class to check that the data is already on the database, the database we using a xml file
```C#
public static void SaveDatabase<T>(T _serialazable, string _fileName)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };
            var writer = XmlWriter.Create(_fileName, settings);

            serializer.WriteObject(writer, _serialazable);
            writer.Close();
        }
```
```XML
<?xml version="1.0" encoding="UTF-8"?>

-<ArrayOfAccountDatabase xmlns="http://schemas.datacontract.org/2004/07/Leder_Efter_Server" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">


-<AccountDatabase>

<active>false</active>

<identity>0</identity>

<password>111</password>

<totalPlay>0</totalPlay>

<totalScore>7</totalScore>

<username>111</username>

</AccountDatabase>


-<AccountDatabase>

<active>false</active>

<identity>0</identity>

<password>123</password>

<totalPlay>3</totalPlay>

<totalScore>16</totalScore>

<username>123</username>

</AccountDatabase>


-<AccountDatabase>

<active>false</active>

<identity>0</identity>

<password>admin</password>

<totalPlay>0</totalPlay>

<totalScore>0</totalScore>

<username>admin</username>

</AccountDatabase>


-<AccountDatabase>

<active>false</active>

<identity>0</identity>

<password>123</password>

<totalPlay>0</totalPlay>

<totalScore>0</totalScore>

<username>ilham</username>

</AccountDatabase>

</ArrayOfAccountDatabase>
```

#### SIgn Up
next to sign in form there is sign up form, when user doesnt have an account to sign in so they can make a new account to sign in, below is code from the sign up function on the clients
```C#
public static void SignUpRequest(string uname, string pass)
    {
        using (Packet _packet = new Packet((int)ClientPackets.signUpRequest))
        {
            ClientData.Account _account = new ClientData.Account(uname, pass);

            _packet.Write<ClientData.Account>(_account);
            SendTCPData(_packet);
        }
    }
```
then client will send packet to server, this packet have identifier called signUpRequest that make server must run SignUpReceived function, in this function user's input will be checked 
```C#
public static void SignUpReceived(int _fromClient, Packet _packet)
        {
            ClientData.Account _account = _packet.ReadObject<ClientData.Account>();
            string validation = AccountHandler.SignUp(_account.username, _account.password);
            ServerSend.SignUpValidation(_fromClient, validation);
        }
```
on SignUpReceiver will run a function SignUp, this function check the user input, this function have a verification to check that username that user have been input already on the database or not, then this function will call AddDataToDatabase
```C#
public static string SignUp(string uname, string pass)
        {
            foreach (AccountDatabase oacc in Server.accountDatabase)
            {
                if (uname == oacc.username)
                {
                    return "login failed! change your username";
                }
            }

            Server.accountDatabase.Add(new AccountDatabase(Client.identity, false, uname, pass, "0", "0"));
            Console.WriteLine($"There's player join: {uname}");

            AddDataToDatabase();
            return "your account registered successfully";
        }
```
this function will check on the xml file, this function will call a function that xml code already write above. then call the SaveDatabase 
```C#
public static void AddDataToDatabase()
        {
            accountDatabaseTemp = LoadDatabase<List<AccountDatabase>>("AccountDatabase.xml");
            accountDatabaseTemp.AddRange(Server.accountDatabase);
            SaveDatabase(Server.accountDatabase, "AccountDatabase.xml");
        }
```
this code is using to open the xml data then write the input from user to the database
```C#
        public static T LoadDatabase<T>(string _fileName)
        {
            var fileStream = new FileStream(_fileName, FileMode.Open);
            var reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
            var serializer = new DataContractSerializer(typeof(T));
            T serializable = (T)serializer.ReadObject(reader, true);

            reader.Close();
            fileStream.Close();
            return serializable;
        }
```
then after that the server will send packet to the client to notify that the new user has been added to the database so user can sign in with the new account
```C#
 public static void SignUpValidation(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.signUp))
            {
                _packet.Write(_msg);
                SendTCPData(_toClient, _packet);
            }
        }
```
