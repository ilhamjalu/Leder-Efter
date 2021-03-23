namespace User
{
    [System.Serializable]
    class Database
    {
        public int identity { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public void setterData(int id, string name, string pass)
        {
            identity = id;
            username = name;
            password = pass;
        }
    }
}