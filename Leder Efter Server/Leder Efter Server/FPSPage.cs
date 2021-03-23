using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExternalMethods
{
    class FPSPage
    {
        public void FPSDataBroadcast(Stream s, List<User.FPSData> ClientFPS)
        {
            foreach (User.FPSData oplayer in ClientFPS)
            {
                IFormatter formatter_send = new BinaryFormatter();
                formatter_send.Binder = new CoreProgram.CustomizedBinder();
                User.FPSData data_send = new User.FPSData();
                data_send.setterData(oplayer.username, oplayer.healthPoint, oplayer.shootAccuracy, oplayer.shoot, oplayer.crouch, oplayer.run);
                formatter_send.Serialize(s, data_send);
            }
        }

        public void FPSDataReceiver(Stream s, List<User.Database> ClientConnected, List<User.FPSData> ClientFPS, int playerId)
        {
            Random random = new Random();
            IFormatter formatter_recv = new BinaryFormatter();
            formatter_recv.Binder = new CoreProgram.CustomizedBinder();
            User.FPSData data_recv = (User.FPSData)formatter_recv.Deserialize(s);
            int shootAccuracy = data_recv.shootAccuracy;

            if (data_recv.shoot)
            {
                shootAccuracy = random.Next(20, 100);
                ClientFPS[playerId].setterData(ClientConnected[playerId].username, ClientFPS[playerId].healthPoint, shootAccuracy, data_recv.shoot, data_recv.crouch, data_recv.run);

                if (shootAccuracy >= 80)
                {
                    foreach (User.FPSData oplayer in ClientFPS)
                    {
                        if (oplayer.username != ClientConnected[playerId].username)
                        {
                            oplayer.healthPoint -= shootAccuracy - 80;
                        }
                    }
                }
            }
            else if (data_recv.crouch)
            {
                ClientFPS[playerId].setterData(ClientConnected[playerId].username, ClientFPS[playerId].healthPoint, data_recv.shootAccuracy, data_recv.shoot, data_recv.crouch, data_recv.run);
            }
            else if (data_recv.run)
            {
                ClientFPS[playerId].setterData(ClientConnected[playerId].username, ClientFPS[playerId].healthPoint, data_recv.shootAccuracy, data_recv.shoot, data_recv.crouch, data_recv.run);
            }
        }

        public void FPSDetailPrint(List<User.FPSData> ClientFPS)
        {
            foreach (User.FPSData oplayer in ClientFPS)
            {
                Console.WriteLine(" " + oplayer.username + " w/ HP: " + 
                                        oplayer.healthPoint + " shoot: " + 
                                        oplayer.shootAccuracy + "%, crouch: " +
                                        oplayer.crouch + " run: " + 
                                        oplayer.run);
            }
        }
    }
}
