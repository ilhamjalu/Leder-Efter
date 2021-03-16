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
}