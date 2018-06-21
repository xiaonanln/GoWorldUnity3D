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
            GameClient.Instance.Connect("ec2-13-229-128-242.ap-southeast-1.compute.amazonaws.com", 15011);
            Console.WriteLine(GameClient.Instance + " created.");

            while (true) {
                GameClient.Instance.Tick();
                Thread.Sleep(100);
            }

            Console.WriteLine("Press Any Key To Quit.");
            Console.ReadKey();
        }
    }
}
