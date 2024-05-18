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

        //Nachricht lesen
        public string ReadString()
        {
            int length = reader.ReadInt32(); 
            byte[] bytes = reader.ReadBytes(length); 
            return Encoding.UTF8.GetString(bytes); 
        }
    }
}
