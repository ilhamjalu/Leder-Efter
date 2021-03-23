using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExternalMethods
{
    class Randomizer
    {
        static string[] team = { "Hijau", "Merah", "Ungu", "Biru" };
        static string[] item = { "Apel", "Anggur", "Jeruk" };

        public static void Randomize(Stream s, StreamWriter writer)
        {
            Random random = new Random();
            int a = random.Next(team.Length);
            int b = random.Next(item.Length);

            IFormatter formatter_send = new BinaryFormatter();
            User.RandomTeam data_send = new User.RandomTeam(team[a]);
            formatter_send.Binder = new CoreProgram.CustomizedBinder();
            formatter_send.Serialize(s, data_send);

            IFormatter formatter_send2 = new BinaryFormatter();
            User.RandomObjective data_send2 = new User.RandomObjective(item[b]);
            formatter_send2.Binder = new CoreProgram.CustomizedBinder();
            formatter_send2.Serialize(s, data_send2);
        }
    }
}