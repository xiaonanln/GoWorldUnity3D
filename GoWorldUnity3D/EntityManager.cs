using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GoWorldUnity3D
{
    public class EntityManager
    {
        internal static EntityManager Instance = new EntityManager();

        const string SPACE_ENTITY_NAME = "__space__";

        Dictionary<string, ClientEntity> entities = new Dictionary<string, ClientEntity>();
        Dictionary<string, Type> entityTypes = new Dictionary<string, Type>();
        public ClientEntity ClientOwner;
        public ClientSpace Space;

        internal ClientEntity CreateEntity(string typeName, string entityID, bool isClientOwner, float x, float y, float z, float yaw, MapAttr attrs)
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
            e.init(entityType, entityID, isClientOwner, x, y, z, yaw, attrs);
            this.entities[entityID] = e;
            e.onCreated();

            // new entity created 
            if (e.IsSpace)
            {
                // enter new space
                if (this.Space != null)
                {
                    this.Space.Destroy();
                }

                this.Space = e as ClientSpace;
                this.onEnterSpace();
            } else
            {
                if (e.IsClientOwner)
                {
                    if (this.ClientOwner != null)
                    {
                        Logger.Warn("EntityManager", "Replacing Existing Player: " + this.ClientOwner);
                        this.ClientOwner.Destroy();
                    }

                    this.ClientOwner = e;
                    e.becomeClientOwner();
                }

                if (this.Space != null)
                {
                    e.enterSpace();
                }
            }
            return e; 
        }

        internal void Update()
        {
            foreach (ClientEntity entity in this.entities.Values)
            {
                entity.update();
            }
        }

        internal ClientEntity getEntity(string entityID)
        {
            try
            {
                return this.entities[entityID]; 
            } catch (KeyNotFoundException)
            {
                return null;
            }
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

        private void onLeaveSpace()
        {
            foreach (ClientEntity entity in this.entities.Values)
            {
                if (!entity.IsSpace)
                {
                    entity.leaveSpace();
                }
            }
        }


        internal void delEntity(ClientEntity e)
        {
            this.entities.Remove(e.ID);
            if (this.Space == e)
            {
                this.Space = null;
                this.onLeaveSpace();
            } else
            {
                if (this.ClientOwner == e)
                {
                    this.ClientOwner = null;
                }

                if (this.Space != null)
                {
                    e.leaveSpace();
                }
            }
        }

        internal void RegisterEntity(Type entityType)
        {
            Debug.Assert(entityType.IsSubclassOf(typeof(ClientEntity)));

            string entityTypeName = entityType.Name;
            Debug.Assert(!this.entityTypes.ContainsKey(entityTypeName) || this.entityTypes[entityTypeName] == entityType);
            this.entityTypes[entityTypeName] = entityType;
        }

        internal void CallEntity(string entityID, string method, object[] args)
        {
            ClientEntity entity; 
            if (this.entities.TryGetValue(entityID, out entity))
            {
                System.Reflection.MethodInfo methodInfo = entity.GetType().GetMethod(method);
                if (methodInfo == null)
                {
                    Logger.Error("EntityManager", "Call Entity {0}.{1}({2} Args) Failed: Public Method Not Found", entity, method, args.Length);
                    return;
                }

                Logger.Debug("EntityManager", "Call Entity {0}: {1}({2} Args)", entity, method, args.Length);
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

        internal void OnSyncEntityInfo(string entityID, float x, float y, float z, float yaw)
        {
            ClientEntity entity;
            try
            {
                entity = this.entities[entityID];
            } catch (KeyNotFoundException)
            {
                Logger.Warn("EntityManager", "Entity {0} Sync Entity Info Failed: Entity Not Found", entityID);
                return;
            }

            entity.OnSyncEntityInfo(x, y, z, yaw);
        }

        internal void OnMapAttrChange(string entityID, ListAttr path, string key, object val)
        {
            ClientEntity entity;
            try
            {
                entity = this.entities[entityID];
            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("EntityManager", "Entity {0} Sync Entity Info Failed: Entity Not Found", entityID);
                return;
            }

            entity.OnMapAttrChange(path, key, val);
        }

        internal void OnMapAttrDel(string entityID, ListAttr path, string key)
        {
            ClientEntity entity;
            try
            {
                entity = this.entities[entityID];
            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("EntityManager", "Entity {0} Sync Entity Info Failed: Entity Not Found", entityID);
                return;
            }

            entity.OnMapAttrDel(path, key);
        }

        internal void OnMapAttrClear(string entityID, ListAttr path)
        {
            ClientEntity entity;
            try
            {
                entity = this.entities[entityID];
            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("EntityManager", "Entity {0} Sync Entity Info Failed: Entity Not Found", entityID);
                return;
            }

            entity.OnMapAttrClear(path);
        }
    }


}
