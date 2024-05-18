using System;
using System.IO;
using System.Text;

namespace server.Net.IO
{
    public class PacketBuilder
    {
        private MemoryStream stream;
        private BinaryWriter writer;

        public PacketBuilder()
        {
            stream = new MemoryStream();            //Zwischenspeicher
            writer = new BinaryWriter(stream);      //schreibt Daten in Zwischenspeicher
        }

        public void WriteOpCode(byte opCode)
        {
            writer.Write(opCode);
        }

        public void WriteString(string value)
        {
            writer.Write(value);
        }

        public byte[] GetPacketBytes()
        {
            return stream.ToArray();
        }
    }
}
