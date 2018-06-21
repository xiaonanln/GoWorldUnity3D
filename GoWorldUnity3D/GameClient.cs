using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace GoWorldUnity3D
{
    public class GameClient
    {
        public static GameClient Instance = new GameClient();
        private TcpClient tcpClient;

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
                this.debug("Available: " + this.tcpClient.Available);
            }
        }

        private void assureTCPClientConnected()
        {
            if (this.tcpClient != null)
            {
                return;
            }

            this.debug("Connecting ...");
            this.tcpClient = new TcpClient();
            this.tcpClient.NoDelay = true;
            this.tcpClient.BeginConnect(this.Host, this.Port, this.onConnected, null);
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
