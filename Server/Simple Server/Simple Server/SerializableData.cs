namespace ClientData
{
    [System.Serializable]
    class SerializableData
    {
        public string name;
        public string pass;

        public SerializableData(string username, string password)
        {
            name = username;
            pass = password;
        }
    }

    [System.Serializable]
    class Player
    {
        public string team;
        public string item;

        public Player(string a, string b)
        {
            team = a;
            item = b;
        }
    }
}