using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leder_Efter_Server
{
    class TriviaHandler
    {
        public static void SetQuestion(bool ready, int maxQuestion)
        {
            int questionResult = 0;

            if (ready)
            {
                Random rand = new Random();
                questionResult = rand.Next(0, maxQuestion);
                ServerSend.TriviaQuestionBroadcast(questionResult);
            }
        }
    }
}
