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
            GoWorld.RegisterEntity(new UnityEngine.GameObject("Account", typeof(Account)));
            GoWorld.RegisterEntity(new UnityEngine.GameObject("Player", typeof(Player)));
            GoWorld.RegisterEntity(new UnityEngine.GameObject("Monster", typeof(Monster)));
            GoWorld.Connect("ec2-13-229-128-242.ap-southeast-1.compute.amazonaws.com", 15011);
            Console.WriteLine(GoWorldUnity3D.GoWorld.GameClient + " created.");

            while (true) {
                GoWorldUnity3D.GoWorld.Update();
                Thread.Sleep(100);
            }
        }

        private static void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - Program - " + msg, args));
        }
    }
}
