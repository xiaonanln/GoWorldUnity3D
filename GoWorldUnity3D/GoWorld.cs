using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoWorld
{
    public class GoWorld
    {
        public static GameClient GameClient = GameClient.Instance;
        public static EntityManager EntityManager = EntityManager.Instance;

        static GoWorld()
        {
            GameClient.OnCreateEntityOnClient += OnCreateEntityOnClient;
        }

        public static void Tick()
        {
            GameClient.Tick();
        }

        public static void Connect(string host, int port)
        {
            GameClient.Connect(host, port);
        }

        public static void OnCreateEntityOnClient(string typeName, string entityID, bool isPlayer, float x, float y, float z, float yaw, Hashtable attrs)
        {
            debug("OnCreateEntityOnClient %s<%s> ...", typeName, entityID);
        }

        private static void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - GoWorld - " + msg, args));
        }

    }

}
