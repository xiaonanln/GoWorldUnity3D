using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;

namespace GoWorldUnity3D
{
    public class GameClient
    {
        public static GameClient Instance = new GameClient();
        private TcpClient tcpClient;
        private DateTime startConnectTime = DateTime.MinValue;
        private PacketReceiver packetReceiver;

        public string Host { get; private set; }
        public int Port { get; private set; }

        public GameClient()
        {
        }

        public override string ToString()
        {
            return "GameClient<" + this.Host + ":" + this.Port + ">";
        }

        public void Connect(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            this.disconnectTCPClient();
        }

        public void Disconnect()
        {
            this.Host = "";
            this.Port = 0;
            this.disconnectTCPClient();
        }

        private void disconnectTCPClient()
        {
            if (this.tcpClient != null)
            {
                this.tcpClient.Close();
                this.tcpClient = null;
                this.packetReceiver = null;
                this.startConnectTime = DateTime.MinValue;
                this.debug("Disconnected");
            }
        }

        private void debug(string msg)
        {
            Console.WriteLine(this + " - " + msg);
        }

        public void Tick()
        {
            if (this.Host == "")
            {
                this.disconnectTCPClient();
            }
            else
            {
                this.assureTCPClientConnected();
                if (this.tcpClient != null && this.tcpClient.Connected && this.tcpClient.Available > 0)
                {
                    this.tryRecvNextPacket();
                }
            }
        }

        private void tryRecvNextPacket()
        {
            this.debug("Available: " + this.tcpClient.Available);
            Packet pkt =  this.packetReceiver.RecvPacket();
            if (pkt != null)
            {
                this.debug("Packet Received: " + pkt);
                this.handlePacket(pkt);
            }
        }

        private void handlePacket(Packet pkt)
        {
            switch (pkt.MsgType)
            {
                case Consts.MT_CREATE_ENTITY_ON_CLIENT:
                    break;
            }
        }

        private void assureTCPClientConnected()
        {
            if (this.tcpClient != null)
            {
                if (!this.tcpClient.Connected)
                {
                    this.checkConnectTimeout();
                }
                return;
            }

            // no tcpClient == not connecting, start new connection ...
            this.debug("Connecting ...");
            this.tcpClient = new TcpClient();
            this.tcpClient.NoDelay = true;
            this.tcpClient.Client.Blocking = false;
            this.tcpClient.SendTimeout = 5000;
            this.tcpClient.ReceiveBufferSize = Consts.MAX_PAYLOAD_LEN + Consts.SIZE_FIELD_SIZE;
            this.startConnectTime = DateTime.Now;
            this.packetReceiver = new PacketReceiver(this.tcpClient);
            this.tcpClient.BeginConnect(this.Host, this.Port, this.onConnected, null);
        }

        private void checkConnectTimeout()
        {
            Debug.Assert(this.tcpClient != null);
            if (DateTime.Now - this.startConnectTime > TimeSpan.FromSeconds(5))
            {
                this.disconnectTCPClient();
            }
        }

        private void onConnected(IAsyncResult ar)
        {
            if (this.tcpClient.Connected)
            {
                this.debug("Connected " + this.tcpClient.Connected);
            }
            else
            {
                this.debug("Connect Failed!");
                this.disconnectTCPClient();
            }
        }
    }
}
