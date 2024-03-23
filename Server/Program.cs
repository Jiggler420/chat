using Server.Net.IO;
using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Programm
    {
        static List<Client> clients;   
        static TcpListener listener;

        static void Main(string[] args)
        {
            clients = new List<Client>();

            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 49459);    //Sucht auf 127.0.0.1:49459 nach eingehenden Verbindungen
            listener.Start();
            
            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());                //Nimmt eingehende Verbindungen an und legt neuen Client an
                clients.Add(client);
            }

            BroadcastConnection();
        }

        //Broadcast dass sich ein neuer Client verbunden hat
        static void BroadcastConnection()
        {
            foreach (var client in clients)
            {
                foreach (var usr in clients)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteString(usr.username);
                    broadcastPacket.WriteString(usr.id.ToString());
                    client.clientSocket.Client.Send(broadcastPacket.GetPacketByte());
                }
            }
        }
    }
}