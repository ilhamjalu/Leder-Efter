namespace User
{
    [System.Serializable]
    class LoginData
    {
        public string name;
        public string pass;

        public LoginData(string username, string password)
        {
            name = username;
            pass = password;
        }
    }
}