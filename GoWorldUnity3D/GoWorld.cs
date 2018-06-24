﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GoWorldUnity3D
{
    public class GoWorld
    {
        public static GameClient GameClient = GameClient.Instance;
        public static EntityManager EntityManager = EntityManager.Instance;
        public static ClientEntity ClientOwner
        {
            get
            {
                return EntityManager.Instance.ClientOwner;
            }
        }

        public static ClientSpace Space
        {
            get
            {
                return EntityManager.Instance.Space;
            }
        }

        static GoWorld()
        {
            GameClient.OnCreateEntityOnClient += OnCreateEntityOnClient;
            GameClient.OnCallEntityMethodOnClient += OnCallEntityMethodOnClient;
            GameObject clientSpace = new GameObject("ClientSpace", typeof(ClientSpace));
            RegisterEntity(clientSpace);
        }

        public static void Update()
        {
            GameClient.Update();
            EntityManager.Update();
        }

        public static void RegisterEntity(UnityEngine.GameObject gameObject)
        {
            EntityManager.RegisterEntity(gameObject);
        }

        public static ClientEntity GetEntity(string entityID)
        {
            return EntityManager.getEntity(entityID);
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
