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
            if (this.tcpClient != null)
            {
                this.tcpClient.Close();
                this.tcpClient = null;
            }
        }

        public void Tick()
        {
            if this.Host == "" {

            }
        }
    }
}
