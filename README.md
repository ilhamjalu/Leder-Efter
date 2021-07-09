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

### Login Page
#### Sign Up
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

### Main Menu
#### Host
main menu have a lot of feature, let me tell you one by one, first is button to play called online, when online button clicked another button will shown, this code is use to shown the button to make a room or join a room
```C#
    public void ChooseOnline(int code)
    {
        if (!touched && code == 1)
        {
            onlineChoice.SetActive(true);
            touched = true;
        }
        else if (touched)
        {
            onlineChoice.SetActive(false);
            touched = false;
        }
    }
```

below is code that run when user click host button, it will create a room to play, and this code will call a function HostRoomRequest
```C#
    public void ChooseHost()
    {
        if (codeRoom.text != "")
        {
            ClientSend.HostRoomRequest(codeRoom.text);
            RoomDatabase.instance.roomCode = codeRoom.text;
        }
        else
        {
            notifText.text = "please input the room code!";
        }
    }
```

on the HostRoomRequest will be send a packet that contain room code to the server
```C#
    public static void HostRoomRequest(string code)
    {
        using (Packet _packet = new Packet((int)ClientPackets.hostRoomRequest))
        {
            _packet.Write(code);
            SendTCPData(_packet);
        }
    }
```

since server receive a hostroomrequest packet data so server must run HostRoomReceived function, then this function will call a validation class
```C#
        public static void HostRoomReceived(int _fromClient, Packet _packet)
        {
            string code = _packet.ReadString();
            string notif = RoomHandler.HostRoomValidation(code);
            ServerSend.HostRoomValidation(_fromClient, notif);
        }
```

then HostRoomValidation will do a validation of a room code that clients send, than on this class will send the code to HostRoom function
```C#
        public static string HostRoomValidation(string code)
        {
            foreach (RoomDatabase oroom in roomDatabase)
            {
                if (code == oroom.code)
                {
                    return "create failed! change your room code or join with that code";
                }
            }

            HostRoom(code);
            return "room created succesfully";
        }
```
on the HostRoom, the code wiil be added to the a list of room that a list of room have a variable code that is room code, and isPlay that will be a condition of the room
```C#
        public static void HostRoom(string code)
        {
            roomDatabase.Add(new RoomDatabase(code));
            Console.WriteLine($"Room Created Succesfully w/ Code: {code}");
        }
```
```C#
    class RoomDatabase
    {
        public string code { get; set; }
        public bool isPlay { get; set; }

        public List<PlayerJoinedDatabase> playerJoinedDatabase = new List<PlayerJoinedDatabase>();

        public RoomDatabase(string _code)
        {
            code = _code;
        }
    }
```
then server will send again the room code to the clients, below is the code
```C#
        public static void HostRoomValidation(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.hostRoom))
            {
                _packet.Write(_msg);
                SendTCPData(_toClient, _packet);
            }
        }
```

#### Join
then when there is already room in there, player can join to the room that available, player can click join, then this class will call JoinRoomRequest to send a packet data that contain username, id and code to the server
```C#
    public void ChooseJoin()
    {
        if (codeRoom.text != "")
        {
            ClientSend.JoinRoomRequest(codeRoom.text, Client.instance.myId, Client.instance.myUname);
            RoomDatabase.instance.roomCode = codeRoom.text;
        }
        else
        {
            notifText.text = "please input the room code!";
        }
    }
```
```C#
    public static void JoinRoomRequest(string code, int id, string uname)
    {
        using (Packet _packet = new Packet((int)ClientPackets.joinRoomRequest))
        {
            _packet.Write(code);
            _packet.Write(id);
            _packet.Write(uname);
            SendTCPData(_packet);
        }
    }
```

then the server will run a function JoinRoomReceived, in this function will be call another function called JoinRoomValidation to do a validation, is there any room with that code or not, if there a room with that code the client can join into a room, but if there is no room with that code player cant join into a room, because there is no room with that code 
```C#
        public static void JoinRoomReceived(int _fromClient, Packet _packet)
        {
            string code = _packet.ReadString();
            int id = _packet.ReadInt();
            string uname = _packet.ReadString();
            string notif = RoomHandler.JoinRoomValidation(code, id, uname);
            ServerSend.JoinRoomValidation(_fromClient, notif);
        }
```
```C#
        public static string JoinRoomValidation(string code, int id, string uname)
        {
            foreach (RoomDatabase oroom in roomDatabase)
            {
                if (code == oroom.code)
                {
                    JoinRoom(code, id, uname);
                    return "joined succesfully";
                }
            }

            return "join failed! change your room code or host a new room";
        }
```
since the client send a code, id and username server will call a function called JoinRoom, in this function the clients will be checked, if the player has join the room player will be broadcasted to all the clients
```C#
        public static void JoinRoom(string _code, int id,string _uname)
        {
            for (int i = 0; i < roomDatabase.Count; i++)
            {
                if (_code == roomDatabase[i].code)
                {
                    bool playerJoined = false;
                    foreach (PlayerJoinedDatabase oplayer in roomDatabase[roomDatabase.Count - 1].playerJoinedDatabase)
                    {
                        if (_uname == oplayer.username)
                            playerJoined = true;
                    }

                    if (!playerJoined)
                    {
                        roomDatabase[i].playerJoinedDatabase.Add(new PlayerJoinedDatabase(id, _uname, 0));

                        for (int j = 0; j < roomDatabase[i].playerJoinedDatabase.Count; j++)
                        {
                            ServerSend.BroadcastPlayerJoined(_code, roomDatabase[i].playerJoinedDatabase[j].id, roomDatabase[i].playerJoinedDatabase[j].username);
                        }
                    }
                }
            }
        }

```
```C#
        public static void BroadcastPlayerJoined(string _codeRoom, int _id, string _uname)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerJoined))
            {
                _packet.Write(_codeRoom);
                _packet.Write(_id);
                _packet.Write(_uname);
                SendTCPDataToAll(_packet);
            }
        }
```

then on the clients side, player who join the room will be added to a list, after that  player who has been join the room will be shown on every clients 
```C#
    public static void AddPlayerToDatabase(Packet _packet)
    {
        string _codeRoom = _packet.ReadString();
        int _id = _packet.ReadInt();
        string _uname = _packet.ReadString();

        if (_codeRoom == RoomDatabase.instance.roomCode)
        {
            RoomDatabase.instance.AddPlayerToDatabase(_id, _uname);
        }
    }
``` 
```C#
    public void AddPlayerToDatabase(int id, string uname)
    {
        bool found = false;
        for (int i = 0; i < playerDatabase.Count; i++)
        {
            if (id == playerDatabase[i].id)
                found = true;
        }

        if (!found)
            playerDatabase.Add(new PlayerDatabase(id, uname, 0));
    }
```
beside play button there is a leave button that will take you out of room and go back to the main menu, in this function client will send packets data that tells the server that user has been leave from the room
```C#
    public void LeaveDestroyRoom()
    {
        if (!Client.instance.isHost)
        {
            ClientSend.LeaveRoomRequest(RoomDatabase.instance.roomCode, Client.instance.myUname);
            RoomDatabase.instance.RemoveDatabase();
        }
        else
        {
            ClientSend.DestroyRoomRequest(RoomDatabase.instance.roomCode);
            GoToScene("MainMenu");
        }

        Client.instance.isPlay = false;
    }
```

and from the play button, client will send a packet to the server to tells the server that the game going to play
```C#
    public void StartMatch(string gameType)
    {
        ClientSend.StartMatch(RoomDatabase.instance.roomCode, gameType);
    }
```
```C#
    public static void StartMatch(string code, string gameType)
    {
        using (Packet _packet = new Packet((int)ClientPackets.startMatchRequest))
        {
            _packet.Write(code);
            _packet.Write(gameType);
            SendTCPData(_packet);
        }
    }
```
when client who is the host of the room send packet data that tell server that game hsa been play, server take that packet and broadcast it to all of the clients 
```C#
        public static void StartMatchReceived(int _fromClient, Packet _packet)
        {
            string code = _packet.ReadString();
            string gameType = _packet.ReadString();
            RoomHandler.StartMatch(code, true, gameType);
            ServerSend.BroadcastStartMatch(code, gameType);
        }
```
```C#
        public static void BroadcastStartMatch(string _codeRoom, string _gameType)
        {
            using (Packet _packet = new Packet((int)ServerPackets.startMatch))
            {
                _packet.Write(_codeRoom);
                _packet.Write(_gameType);
                SendTCPDataToAll(_packet);
            }
        }
```

### GamePlay
after the host play the game, all of clients will be sent to the gameplay scene, first on the gameplay will be generated player as much as clients whose join the room, player script has a function to do a movement script
```C#
public class PlayerCharManager : MonoBehaviour
{
	public int horizontal = 0;
	public int vertical = 0;
	public float maxSpeed = 5f;

	public Animator anim;
	public Rigidbody2D rb;

	public bool faceRight = true;
	public bool isDead = false;
	
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = gameObject.GetComponent<Animator>();
		anim.SetBool("walk", false);
		anim.SetBool("dead", false);
		anim.SetBool("jump", false);
	}

    void FixedUpdate()
    {
		Move(horizontal, vertical);
	}

	void Move(int horizontal, int vertical)
    {
		if (horizontal > 0)
		{
			anim.SetBool("walk", true);
			if (faceRight == false)
			{
				Flip();
			}
		}

		if ((horizontal < 0))
		{
			anim.SetBool("walk", true);
			if (faceRight == true)
			{
				Flip();
			}
		}

		if (vertical > 0)
			anim.SetBool("walk", true);

		if (vertical < 0)
			anim.SetBool("walk", true);

		if (horizontal == 0 && vertical == 0)
			anim.SetBool("walk", false);

		rb.velocity = new Vector3(horizontal * maxSpeed, vertical * maxSpeed, 0);

	}

	void Flip()
	{
		faceRight = !faceRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
```
a Game Manager Script will send player movement to the server
```C#
    public void MoveRequest()
    {
        int horizontal = Convert.ToInt32(Input.GetAxis("Horizontal"));
        int vertical = Convert.ToInt32(Input.GetAxis("Vertical"));

        if (horizontal == 0 && vertical == 0 && !sendIdle) {
            ClientSend.PlayerPosition(RoomDatabase.instance.roomCode,
                                      Client.instance.myId, horizontal, vertical);
            
            sendIdle = true;
        }
        
        if (horizontal != 0 || vertical != 0)
        {
            ClientSend.PlayerPosition(RoomDatabase.instance.roomCode,
                                      Client.instance.myId, horizontal, vertical);

            sendIdle = false;
        }
    }

    public void UpdatePlayerPosition(int id, int _controlHorizontal, int _controlVertical)
    {
        for (int i = 0; i < RoomDatabase.instance.playerDatabase.Count; i++)
        {
            if (RoomDatabase.instance.playerDatabase[i].id == id)
            {
                RoomDatabase.instance.playerDatabase[i].character.horizontal = _controlHorizontal;
                RoomDatabase.instance.playerDatabase[i].character.vertical = _controlVertical;
            }
        }
    }
```

then the server will receive the player position then broadcast it to another player, so they cant see eacb other position
```C#
        public static void PlayerPositionReceived(int _fromClient, Packet _packet)
        {
            string code = _packet.ReadString();
            int id = _packet.ReadInt();
            int controlHorizontal = _packet.ReadInt();
            int controlVertical = _packet.ReadInt();
            
            //Console.WriteLine($"{code} - {id} - {controlHorizontal} - {controlVertical}");
            ServerSend.BroadcastPlayerPosition(code, id, controlHorizontal, controlVertical);
        }
	
	public static void BroadcastPlayerPosition(string _codeRoom, int _id, int _controlHorizontal, int _controlVertical)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
            {
                _packet.Write(_codeRoom);
                _packet.Write(_id);
                _packet.Write(_controlHorizontal);
                _packet.Write(_controlVertical);
                SendTCPDataToAll(_packet);
            }
        }
```
below is code to set the question, from all the question will be choose 10 to show for the client, and below code have a function to count player score and show game over if the all of question already finished 
```C#
    public void SetDatabase(string codeRoom, int categoryResult, int questionResult)
    {
        if (RoomDatabase.instance.roomCode == codeRoom) {
            if (categoryResult == 0)
                trivia.Add(TriviaDatabase.instance.triviaHewan[questionResult]);
            else if (categoryResult == 1)
                trivia.Add(TriviaDatabase.instance.triviaTumbuhan[questionResult]);
            else if (categoryResult == 2)
                trivia.Add(TriviaDatabase.instance.triviaNegara[questionResult]);
            else if (categoryResult == 3)
                trivia.Add(TriviaDatabase.instance.triviaDunia[questionResult]);
        }
    }

    public void SetQuestion(string codeRoom, int questionResult)
    {
        if (RoomDatabase.instance.roomCode == codeRoom) {
            questionTemp = questionResult;
            questionText.text = $"{trivia[questionResult].question}";
            questionFix = trivia[questionResult].question;
            answerFix = trivia[questionResult].answer;
            StartCoroutine(QuestionCountDown());
        }
    }

    IEnumerator QuestionCountDown()
    {
        questionCountDown -= 1;
        yield return new WaitForSeconds(1);

        if (questionCountDown > 0)
            StartCoroutine(QuestionCountDown());
        else
        {
            trivia.RemoveAt(questionTemp);
            questionCountDown = 5f;
            questionTemp = 0;

            if (answer)
            {
                score++;
                answer = false;
            }

            if (Client.instance.isHost)
                ClientSend.TriviaRequest(RoomDatabase.instance.roomCode);

            if (trivia.Count == 0)
            {
                Client.instance.myScore += score;
                Client.instance.myPlay++;

                ClientSend.UpScorePlay(Client.instance.myUname, 
                                       Client.instance.myScore,
                                       Client.instance.myPlay);

                Debug.Log($"Total Score: {Client.instance.myScore}");
                Debug.Log($"Total Play: {Client.instance.myPlay}");

                gameOverPanel.SetActive(true);
                scoreFinalText.text = $"Your Score's: {score}";
                isPlay = false;
            }
        }
    }
```
every answer will be cheked by code below
```C#
public class TriviaCheckAnswer : MonoBehaviour
{
    public string thisAnswer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == Client.instance.myUname)
        {
            if (thisAnswer == UITriviaManager.instance.answerFix)
            {
                UITriviaManager.instance.answer = true;
                Debug.Log("Jawabanmu Benar");
            }
            else
            {
                UITriviaManager.instance.answer = false;
                Debug.Log("Jawabanmu Salah");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == Client.instance.myUname)
        {
            UITriviaManager.instance.answer = false;
            Debug.Log("Kembali Ke Jawabanmu!");
        }
    }
}
```
the server will be choose 10 question randomly
```C#
    class TriviaHandler
    {
        public static void SetQuestion(string codeRoom, bool ready, int maxQuestion)
        {
            int questionResult = 0;

            if (ready)
            {
                Random rand = new Random();
                questionResult = rand.Next(0, maxQuestion);
                ServerSend.TriviaQuestionBroadcast(codeRoom, questionResult);
            }
        }

        public static void SetDatabase(string codeRoom, int maxCategory, int maxQuestion)
        {
            List<int> numberTemp = new List<int>();

            for (int i = 0; i < maxQuestion; i++)
            {
                int categoryResult = 0;
                int questionResult = 0;

                Random rand = new Random(DateTime.Now.Millisecond);

                do
                {
                    categoryResult = rand.Next(0, maxCategory);
                    questionResult = rand.Next(0, maxQuestion);
                } while (numberTemp.Contains(questionResult));
                numberTemp.Add(questionResult);                                

                //Console.WriteLine($"Category: {categoryResult} - Question: {questionResult}");
                ServerSend.TriviaDatabaseBroadcast(codeRoom, categoryResult, questionResult);
            }
        }
    }
```
