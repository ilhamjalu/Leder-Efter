using System;
using System.Collections.Generic;
using System.Text;

namespace Leder_Efter_Server
{
    class RandomizeDatabase
    {
        public static List<string> stuff = new List<string> { "Grape", "Banana", "Apple" };
        public static List<string> color = new List<string> { "Merah", "Hijau", "Biru" };
    }

    class RandomizeHandler
    {
        public static string StuffRandomizer()
        {
            Random random = new Random();
            int index = random.Next(RandomizeDatabase.stuff.Count);
            var result = RandomizeDatabase.stuff[index];
            RandomizeDatabase.stuff.RemoveAt(index);
            return result;
        }

        public static string ColorRandomizer()
        {
            Random random = new Random();
            int index = random.Next(RandomizeDatabase.color.Count);
            var result = RandomizeDatabase.color[index];
            RandomizeDatabase.color.RemoveAt(index);
            return result;
        }
    }
}
