namespace User
{
    [System.Serializable]
    class LoginData
    {
        public int identity;
        public string username;
        public string password;

        public LoginData(int id, string uname, string pass)
        {
            this.identity = id;
            this.username = uname;
            this.password = pass;
        }

        public string Getter()
        {
            return this.username + " | " + this.password;
        }
    }
}