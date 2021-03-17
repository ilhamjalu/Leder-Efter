using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExternalMethods
{
    class Randomizer
    {
        static string[] team = { "Hijau", "Merah", "Ungu" };
        static string[] item = { "Apel", "Anggur", "Jeruk" };

        public static void Randomize(Stream s)
        {
            Random random = new Random();
            int a = random.Next(team.Length);
            int b = random.Next(item.Length);

            IFormatter formatter_send = new BinaryFormatter();
            formatter_send.Binder = new CoreProgram.CustomizedBinder();
            User.RandomObjective data_send = new User.RandomObjective(item[a]);
            formatter_send.Serialize(s, data_send);

            formatter_send = new BinaryFormatter();
            formatter_send.Binder = new CoreProgram.CustomizedBinder();
            User.RandomTeam data_send2 = new User.RandomTeam(team[b]);
            formatter_send.Serialize(s, data_send2);
        }
    }
}