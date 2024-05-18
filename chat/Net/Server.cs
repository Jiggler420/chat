using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using client.Net.IO;
using System.Windows.Documents;

namespace client.Net
{
    class Server
    {
        TcpClient tcpClient; //Aufsetzen eines TCP-Clients
        PacketBuilder packetBuilder;

        public PacketReader PacketReader;
        
        public event Action connected;      //Event für die erfolgreiche Verbindung
        public event Action msgRecieved;    //Event für eine empfangene Nachricht
        public event Action disconnected;   //Event für eine verlorene Verbindung 

        public Server() { 
            tcpClient = new TcpClient(); //initalisieren 
        }

        public void ConnectToServer(string username)
        {
            try
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
                        tcpClient.Client.Send(connectPacket.GetPacketBytes());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Verbinden zum Server: {ex.Message}");
            }
        }


        private void ReadPackets()
        {
            //Prüfen ob der Stream noch Daten enthält bevor der Opcode interpretiert wird
            Task.Run(() =>
            {
                try
                {
                    if (tcpClient.Available > 0)                                                  //Prüfe ob Daten vorhanden sind
                    {
                        var opcode = PacketReader.ReadByte();
                        switch (opcode)                                                          //Mache das was der Opcode dir sagt
                        {
                            case 1:
                                connected?.Invoke();
                                break;

                            case 5:
                                msgRecieved?.Invoke();
                                break;

                            case 10:
                                disconnected?.Invoke();
                                break;

                            default:
                                Console.WriteLine("FEHLER: Opcode nicht bekannt" + opcode);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"---Stream beendet---");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteString(message);
            tcpClient.Client.Send(messagePacket.GetPacketBytes());
        }
    }

}
