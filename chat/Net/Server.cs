using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using client.Net.IO;

namespace client.Net
{
    class Server
    {
        TcpClient tcpClient; //Aufsetzen eines TCP-Clients
        PacketBuilder packetBuilder;

        public PacketReader PacketReader;
        
        public event Action connected;  //Event für die erfolgreiche Verbindung

        public Server() { 
            tcpClient = new TcpClient(); //initalisieren 
        }

        public void ConnectToServer(string username)        //Verbinden
        {
            if (!tcpClient.Connected)
            {
                tcpClient.Connect("127.0.0.1", 49459);
                var connectPacket = new PacketBuilder();

                if (!string.IsNullOrEmpty(username)) 
                {
                    PacketReader = new PacketReader(tcpClient.GetStream());
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteString(username);
                    tcpClient.Client.Send(connectPacket.GetPacketByte());
                }

                ReadPackets();
            }
        } 
        
        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connected?.Invoke();
                            break;
                        default:
                            Console.WriteLine("...___...");
                            break;
                            
                    }
                }
            });
        }
    }
}
