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
            GameClient.OnCallEntityMethodOnClient += OnCallEntityMethodOnClient;
            RegisterEntity(typeof(ClientSpace));
        }

        public static void Tick()
        {
            GameClient.Tick();
        }

        public static void RegisterEntity(Type entityType)
        {
            EntityManager.RegisterEntity(entityType);
        }

        public static void Connect(string host, int port)
        {
            GameClient.Connect(host, port);
        }

        private static void OnCreateEntityOnClient(string typeName, string entityID, bool isClientOwner, float x, float y, float z, float yaw, MapAttr attrs)
        {
            debug("OnCreateEntityOnClient {0}<{1}>, IsClientOwner={2}, Attrs={3} ...", typeName, entityID, isClientOwner, attrs);
            ClientEntity e = EntityManager.CreateEntity(typeName, entityID, isClientOwner, x, y, z, yaw, attrs);
        }

        private static void OnCallEntityMethodOnClient(string entityID, string method, object[] args)
        {
            EntityManager.CallEntity(entityID, method, args);
        }

        private static void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - GoWorld - " + msg, args));
        }

    }

}
