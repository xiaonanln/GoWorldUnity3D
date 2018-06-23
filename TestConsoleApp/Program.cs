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
            GoWorldUnity3D.GoWorld.RegisterEntity(typeof(Account));
            GoWorldUnity3D.GoWorld.RegisterEntity(typeof(Player));
            GoWorldUnity3D.GoWorld.RegisterEntity(typeof(Monster));
            GoWorldUnity3D.GoWorld.Connect("ec2-13-229-128-242.ap-southeast-1.compute.amazonaws.com", 15011);
            Console.WriteLine(GoWorldUnity3D.GoWorld.GameClient + " created.");

            while (true) {
                GoWorldUnity3D.GoWorld.Update();
                Thread.Sleep(100);
            }

            Console.WriteLine("Press Any Key To Quit.");
            Console.ReadKey();
        }

        private static void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - Program - " + msg, args));
        }
    }
}
