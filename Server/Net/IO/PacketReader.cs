using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Net.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream _networkStream;
        public PacketReader(NetworkStream networkStream) : base(networkStream)  
        {
            _networkStream = networkStream;
        }

        public string ReadMessage()
        {
            byte[] buffer;
            var length = ReadInt32();
            buffer = new byte[length];
            _networkStream.Read(buffer, 0, length);

            var msg = Encoding.UTF8.GetString(buffer);

            return msg;
        }
    }
}
