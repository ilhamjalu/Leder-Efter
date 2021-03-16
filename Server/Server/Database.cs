namespace Database
{
    [System.Serializable]
    class Client
    {
        public long identity { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string message { get; set; }

        public void setterData(long id, string name, string pass)
        {
            identity = id;
            username = name;
            password = pass;
        }
    }
}