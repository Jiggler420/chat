using System;
using System.IO;
using System.Text;

namespace server.Net.IO
{
    public class PacketReader
    {
        private BinaryReader reader;

        public PacketReader(Stream stream)
        {
            reader = new BinaryReader(stream);
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public string ReadString()
        {
            int length = reader.ReadInt32(); // Länge des Strings lesen
            byte[] bytes = reader.ReadBytes(length); // Bytes des Strings lesen
            return Encoding.UTF8.GetString(bytes); // Bytes in einen String umwandeln
        }
    }
}
