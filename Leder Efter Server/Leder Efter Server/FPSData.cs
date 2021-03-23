namespace User
{
    [System.Serializable]
    class FPSData
    {
        public string username;
        public int healthPoint;
        public int shootAccuracy;
        public bool shoot;
        public bool crouch;
        public bool run;

        public void setterData(string uname, int health, int acc, bool s, bool c, bool r)
        {
            this.username = uname;
            this.healthPoint = health;
            this.shootAccuracy = acc;
            this.shoot = s;
            this.crouch = c;
            this.run = r;
        }
    }
}