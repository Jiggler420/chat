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

            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 49459);
            listener.Start();

            while (true)
            {
                var clientSocket = listener.AcceptTcpClient();
                var client = new Client(clientSocket);
                clients.Add(client);
                BroadcastConnection(); // Hier Broadcast nachdem ein neuer Client verbunden wurde
                BroadcastMessage("Ein neuer Client ist verbunden."); // Beispiel für Broadcast-Nachricht
            }
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
                    client.clientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (var client in clients)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteString(message);
                client.clientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }


        public static void BroadcastDisconnectMessage(string id)
        {
            var disconnectedClient = clients.FirstOrDefault(x => x.id.ToString() == id);
            if (disconnectedClient != null)
            {
                clients.Remove(disconnectedClient);

                    var discPacket = new PacketBuilder();
                    discPacket.WriteOpCode(10);
                    discPacket.WriteString(id);
                    var discPacketBytes = discPacket.GetPacketBytes();

                foreach (var client in clients)
                {
                    client.clientSocket.Client.Send(discPacketBytes);
                }
                BroadcastMessage($"[{disconnectedClient.username}]: Verbindung verloren");
            }
        }
    }
}