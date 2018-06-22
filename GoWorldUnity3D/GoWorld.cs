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

        public delegate void OnEntityCreatedHandler(ClientEntity entity);
        public delegate void OnEntityDestroyHandler(ClientEntity entity);
        public delegate void OnBecomePlayerHandler(ClientEntity entity);

        public static OnEntityCreatedHandler OnEntityCreated;
        public static OnEntityDestroyHandler OnEntityDestroy;
        public static OnBecomePlayerHandler OnBecomePlayer;

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
            debug("OnCreateEntityOnClient {0}<{1}>, isPlayer={2}, attrs={3} ...", typeName, entityID, isPlayer, attrs);

            if (typeName == "__space__")
            {
                OnEnterSpace(entityID, attrs);
                return;
            }

            ClientEntity e = EntityManager.CreateEntity(typeName, entityID, isPlayer, x, y, z, yaw, attrs);
            if (OnEntityCreated != null)
            {
                OnEntityCreated(e);
            }
            if (e.IsPlayer && OnBecomePlayer != null)
            {
                OnBecomePlayer(e);
            }
        }

        private static void OnEnterSpace(string entityID, Hashtable attrs)
        {
            throw new NotImplementedException();
        }

        private static void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - GoWorld - " + msg, args));
        }

    }

}
