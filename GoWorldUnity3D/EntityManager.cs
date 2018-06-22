using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GoWorld
{
    public class EntityManager
    {
        internal static EntityManager Instance = new EntityManager();

        const string SPACE_ENTITY_NAME = "__space__";

        Dictionary<string, ClientEntity> entities = new Dictionary<string, ClientEntity>();
        Dictionary<string, Type> entityTypes = new Dictionary<string, Type>();
        ClientEntity ClientOwner;
        ClientSpace Space;

        internal ClientEntity CreateEntity(string typeName, string entityID, bool isClientOwner, float x, float y, float z, float yaw, Hashtable attrs)
        {
            if (typeName == SPACE_ENTITY_NAME)
            {
                typeName = "ClientSpace";
            }

            Debug.Assert(this.entityTypes.ContainsKey(typeName));

            if (this.entities.ContainsKey(entityID))
            {
                ClientEntity old = this.entities[entityID];
                Logger.Warn("EntityManager", "Entity {0} Already Exists, Destroying Old One: {1}", entityID, old);
                old.Destroy();
            }

            // create new entity of specified type 
            Type entityType = this.entityTypes[typeName];
            ClientEntity e = Activator.CreateInstance(entityType) as ClientEntity;
            this.entities[entityID] = e;
            e.init(entityType, entityID, isClientOwner, x, y, z, yaw, attrs);
            
            if (e.IsClientOwner)
            {
                if (this.ClientOwner != null)
                {
                    Logger.Warn("EntityManager", "Replacing Existing Player: " + this.ClientOwner);
                    this.ClientOwner.Destroy();
                }

                this.ClientOwner = e;
                e.becomeClientOwner();
            } else if (e.IsSpace)
            {
                // enter new space
                if (this.Space != null)
                {
                    this.Space.Destroy();
                }

                this.Space = e as ClientSpace;
                this.onEnterSpace();
            }
            return e; 
        }

        private void onEnterSpace()
        {
            foreach (ClientEntity entity in this.entities.Values)
            {
                if (!entity.IsSpace)
                {
                    entity.enterSpace();
                }
            }
        }

        internal void delEntity(ClientEntity e)
        {
            if (this.ClientOwner == e)
            {
                this.ClientOwner = null;
            } else if (this.Space == e)
            {
                this.Space = null;
                this.onLeaveSpace();
            }

            this.entities.Remove(e.ID);
        }

        private void onLeaveSpace()
        {
            throw new NotImplementedException();
        }

        internal void RegisterEntity(Type entityType)
        {
            Debug.Assert(entityType.IsSubclassOf(typeof(ClientEntity)));

            string entityTypeName = entityType.Name;
            Debug.Assert(!this.entityTypes.ContainsKey(entityTypeName));
            this.entityTypes[entityTypeName] = entityType;
        }

        internal void CallEntity(string entityID, string method, object[] args)
        {
            Logger.Debug("EntityManager", "Call Entity {0}: {1}({2} Args)", entityID, method, args.Length);
            ClientEntity entity; 
            if (this.entities.TryGetValue(entityID, out entity))
            {
                System.Reflection.MethodInfo methodInfo = entity.GetType().GetMethod(method);
                if (methodInfo == null)
                {
                    Logger.Error("EntityManager", "Call Entity {0}.{1}({2} Args) Failed: Public Method Not Found", entityID, method, args.Length);
                    return;
                }

                methodInfo.Invoke(entity, args);
            } else
            {
                // entity not found
                Logger.Error("EntityManager", "Call Entity {0}.{1}({2} Args) Failed: Entity Not Found", entityID, method, args.Length);
            }
        }

        internal void DestroyEntity(string entityID)
        {
            Logger.Debug("EntityManager", "Destroy Entity {0}", entityID);
            ClientEntity entity;
            if (this.entities.TryGetValue(entityID, out entity)) {
                entity.Destroy();
            } else
            {
                Logger.Error("EntityManager", "Destroy Entity {0} Failed: Entity Not Found", entityID);
            }
        }
    }


}
