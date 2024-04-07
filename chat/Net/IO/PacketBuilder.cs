using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace client.Net.IO
{
    public class PacketBuilder
    {
        private MemoryStream stream;
        private BinaryWriter writer;

        public PacketBuilder()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
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
