using System.Net.Sockets;
using server.Net.IO;

namespace Server
{
    class Client
    {
        public string username { get; set; }
        public static List<Client> clients { get; private set; } = new List<Client>();

        public Guid id { get; set; }
        public TcpClient clientSocket;

        PacketReader packetReader;

        public Client(TcpClient client)
        {
            clientSocket = client;
            id = Guid.NewGuid();
            packetReader = new PacketReader(clientSocket.GetStream());


            if (packetReader != null)       //Prüfe ob Packet Reader da ist 
            {
                var opcode = packetReader.ReadByte();
                switch (opcode)
                {
                    case 1:
                        var username = packetReader.ReadString();
                        Console.WriteLine($"[{DateTime.Now}] Neuer Client -- {username} -- verbunden");
                        break;
                    case 5:
                        var msg = packetReader.ReadString();
                        Console.WriteLine($"[{DateTime.Now}]: {msg}");
                        Programm.BroadcastMessage(msg);
                        break;
                    case 10:
                        var id = packetReader.ReadString();
                        Programm.BroadcastDisconnectMessage(id);
                        break;
                    default:
                        Console.WriteLine("FEHLER: Opcode nicht bekannt" + opcode);
                        break;
                }
            }

            Console.WriteLine($"[{DateTime.Now}] Neuer Client -- {username} -- verbunden");     //Ansage für neu verbundene Clients

            Task.Run(() => Process());
        }

        void Process()
        {
            try
            {
                while (clientSocket.Connected)
                {
                    var opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = packetReader.ReadString();
                            Console.WriteLine($"[{DateTime.Now}]: {msg}");
                            Programm.BroadcastMessage(msg);
                            break;
                        default:
                            break;
                    }
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine($"[{id}]: Verbindung verloren - {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
                clients.Remove(this);
            }
        }
    }
}