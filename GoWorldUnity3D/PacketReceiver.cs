﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace GoWorldUnity3D
{
    class PacketReceiver
    {
        private TcpClient tcpClient;
        //private NetworkStream netStream;
        private uint recvPayloadLen;
        private RecvState recvState = RecvState.receivingPayloadLen;
        private byte[] recvPayloadLenBuff;
        private byte[] recvPayloadBuff;

        enum RecvState
        {
            receivingPayloadLen,
            receivingPayload
        };

        public PacketReceiver(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            //this.netStream = tcpClient.GetStream();
            this.recvPayloadLenBuff = new byte[Consts.SIZE_FIELD_SIZE];
        }

        internal Packet RecvPacket() 
        {
            if (this.tcpClient.Available == 0)
            {
                return null;
            }
            int nr;
            if (this.recvState == RecvState.receivingPayloadLen)
            {
                if (tcpClient.Available < Consts.SIZE_FIELD_SIZE)
                {
                    return null;
                }

                nr = tcpClient.Client.Receive(this.recvPayloadLenBuff);
                Debug.Assert(nr == Consts.SIZE_FIELD_SIZE);
                
                this.recvPayloadLen = BitConverter.ToUInt32(recvPayloadLenBuff, 0);
                if (this.recvPayloadLen < 2 || this.recvPayloadLen > Consts.MAX_PAYLOAD_LEN)
                {
                    Console.WriteLine("Invalid Packet Payload Length: " + this.recvPayloadLen);
                    this.tcpClient.Close();
                    return null;
                }
                this.recvState = RecvState.receivingPayload;
                Console.WriteLine("Read Packet Payload Length: " + this.recvPayloadLen);
                this.recvPayloadBuff = new byte[this.recvPayloadLen];
            } 

            if (tcpClient.Available < this.recvPayloadLen)
            {
                return null;
            }

            nr = tcpClient.Client.Receive(this.recvPayloadBuff);
            Debug.Assert(nr == this.recvPayloadLen);
            this.recvState = RecvState.receivingPayloadLen;
            byte[] payload = this.recvPayloadBuff;
            this.recvPayloadBuff = null;
            return new Packet(payload);
        }
    }
}
