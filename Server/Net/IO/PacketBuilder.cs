using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Net.IO
{
    class PacketBuilder
    {
        MemoryStream ms;
        public PacketBuilder()
        {
            ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            ms.WriteByte(opcode);
        }

        public void WriteString(string msg)
        {
            var msgLength = msg.Length;
            ms.Write(BitConverter.GetBytes(msgLength));     //Bytes aus dem String msg in den MemoryStream
            ms.Write(Encoding.ASCII.GetBytes(msg));         //Speichert die ascii-zeichen aus dem string msg in den MemoryStream
        }

        public byte[] GetPacketByte()
        {
            return ms.ToArray();
        }
    }
}
