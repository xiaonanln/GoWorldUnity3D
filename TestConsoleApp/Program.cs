using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoWorldUnity3D;
using System.Threading;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GoWorldUnity3D.GameClient client = new GoWorldUnity3D.GameClient("ec2-13-229-128-242.ap-southeast-1.compute.amazonaws.com", 15011);
            Console.WriteLine(client.ToString() + " created.");

            while (true) {
                client.Tick();
                Thread.Sleep(100);
            }

            Console.WriteLine("Press Any Key To Quit.");
            Console.ReadKey();
        }
    }
}
