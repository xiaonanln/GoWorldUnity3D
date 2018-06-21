using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace GoWorldUnity3D
{
    public class GameClient
    {
        private TcpClient tcpClient;

        public string Host { get; }
        public int Port { get; }

        public GameClient(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public override string ToString()
        {
            return "GameClient<"+this.Host+":"+this.Port+">";
        }

        public void Connect()
        {
            this.tcpClient = new TcpClient(this.Host, this.Port);
            Console.WriteLine("GameClient Connected: " + this);
        }

        public void Tick()
        {

        }
    }
}
