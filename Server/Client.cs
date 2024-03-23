using Server.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        public string username { get; set; }
        public Guid id { get; set; }
        public TcpClient clientSocket;

        PacketReader packetReader;

        public Client (TcpClient client)
        {
            clientSocket = client;
            id = Guid.NewGuid();
            packetReader = new PacketReader(clientSocket.GetStream());

            var opcode = packetReader.ReadByte();
            username = packetReader.ReadString();
            
            Console.WriteLine($"[{DateTime.Now}] Neuer Client -- {username} -- verbunden");     //Ansage für neu verbundene Clients
        }

    }
}