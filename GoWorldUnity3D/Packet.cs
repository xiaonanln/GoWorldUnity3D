using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GoWorld
{
    class Packet
    {
        private byte[] payload;
        public UInt16 MsgType { get; private set; }
        public int UnreadPayloadLen {
            get {
                return this.payload.Length - this.readPos;
            }
        }

        private int readPos, writePos;

        public Packet(byte[] payload)
        {
            this.payload = payload;
            this.MsgType = BitConverter.ToUInt16(this.payload, 0);
            this.readPos = 2;
        }

        public Packet(UInt16 msgtype)
        {
            byte[] b =  BitConverter.GetBytes(msgtype);
            this.payload = b;
            this.writePos = 2;
        }

        public override string ToString()
        {
            return "Packet<" + this.MsgType + "|" + this.payload.Length + ">";
        }

        internal UInt16 ReadUInt16()
        {
            this.assureUnreadPayloadLen(sizeof(UInt16));
            UInt16 res = BitConverter.ToUInt16(this.payload, this.readPos);
            this.readPos += sizeof(UInt16);
            return res; 
        }

        internal uint ReadUInt32()
        {
            this.assureUnreadPayloadLen(sizeof(UInt32));
            UInt32 res = BitConverter.ToUInt32(this.payload, this.readPos);
            this.readPos += sizeof(UInt32);
            return res;
        }

        internal float ReadFloat32()
        {
            this.assureUnreadPayloadLen(sizeof(float));
            float v = BitConverter.ToSingle(payload, readPos);
            this.readPos += sizeof(float);
            return v;
        }

        internal byte[] ReadBytes(int len)
        {
            this.assureUnreadPayloadLen(len);
            byte[] bytes = new byte[len];
            System.Array.Copy(this.payload, this.readPos, bytes, 0, len);
            this.readPos += len;
            return bytes; 
        }

        private void AppendBytes(byte[] v)
        {
            throw new NotImplementedException();
        }

        internal void AppendVarStr(string method)
        {
            throw new NotImplementedException();
        }

        internal void AppendArgs(object[] args)
        {
            throw new NotImplementedException();
        }

        internal string ReadStr(int len)
        {
            byte[] bytes = this.ReadBytes(len);
            return ASCIIEncoding.ASCII.GetString(bytes);
        }

        private void AppendStr(string s)
        {
            this.AppendBytes(ASCIIEncoding.ASCII.GetBytes(s));
        }

        private void assureUnreadPayloadLen(int len)
        {
            Debug.Assert(this.UnreadPayloadLen >= len);
        }

        internal bool ReadBool()
        {
            return this.ReadByte() != 0;
        }

        private byte ReadByte()
        {
            this.assureUnreadPayloadLen(1);
            byte b = this.payload[this.readPos];
            this.readPos += 1;
            return b;
        }

        internal string ReadEntityID()
        {
            return this.ReadStr(Proto.ENTITYID_LENGTH);
        }

        internal void AppendEntityID(string entityID)
        {
            Debug.Assert(entityID.Length == Proto.ENTITYID_LENGTH);
            this.AppendStr(entityID);
        }

        internal string ReadVarStr()
        {
            UInt32 len = this.ReadUInt32();
            return this.ReadStr((int)(len));
        }

        internal byte[] ReadVarBytes()
        {
            UInt32 len = this.ReadUInt32();
            return this.ReadBytes((int)len);
        }

        internal object ReadData()
        {
            byte[] b = this.ReadVarBytes();
            return DataPacker.UnpackData(b);
        }
    }
}
