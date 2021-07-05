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
#### Login Page
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
