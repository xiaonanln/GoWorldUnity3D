using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoWorldUnity3D
{
    class Packet
    {
        private byte[] payload;
        public ushort MsgType { get; private set; }
        private int readPos;

        public Packet(byte[] payload)
        {
            this.payload = payload;
            this.MsgType = BitConverter.ToUInt16(this.payload, 0);
            this.readPos = 2;
        }

        public override string ToString()
        {
            return "Packet<" + this.MsgType + "|" + this.payload.Length + ">";
        }

    }
}
