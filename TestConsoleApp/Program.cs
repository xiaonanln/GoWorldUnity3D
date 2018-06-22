using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoWorld;
using System.Threading;

namespace TestConsoleApp
{
    class Program
    {
        const string USERNAME = "unity3dlib";
        const string PASSWORD = "unity3dlib";
        static void Main(string[] args)
        {
            GoWorld.GoWorld.Connect("ec2-13-229-128-242.ap-southeast-1.compute.amazonaws.com", 15011);
            Console.WriteLine(GoWorld.GoWorld.GameClient + " created.");
            GoWorld.GoWorld.OnEntityCreated += OnEntityCreated;
            GoWorld.GoWorld.OnBecomePlayer += OnBecomePlayer;

            while (true) {
                GoWorld.GoWorld.Tick();
                Thread.Sleep(100);
            }

            Console.WriteLine("Press Any Key To Quit.");
            Console.ReadKey();
        }

        static void OnEntityCreated(ClientEntity e)
        {
            debug("Entity {0} Created!", e);
        }

        static void OnBecomePlayer(ClientEntity e)
        {
            GoWorld.Logger.Info("Program", "OnBecomePlayer: " + e);

            if (e.TypeName == "Account")
            {
                // Account created, logging 
                e.CallServer("Register", USERNAME, PASSWORD);
            }
        }

        

        private static void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - Program - " + msg, args));
        }
    }
}
