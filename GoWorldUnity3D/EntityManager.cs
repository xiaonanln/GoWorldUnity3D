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
        Dictionary<string, ClientSpace> spaces = new Dictionary<string, ClientSpace>();
        ClientEntity Player;

        internal ClientEntity CreateEntity(string typeName, string entityID, bool isPlayer, float x, float y, float z, float yaw, Hashtable attrs)
        {
            Debug.Assert(typeName != SPACE_ENTITY_NAME);
            if (this.entities.ContainsKey(entityID))
            {
                ClientEntity old = this.entities[entityID];
                debug("Entity {0} Already Exists, Destroying Old One: {1}", entityID, old);
                this.entities.Remove(entityID);
                if (old == this.Player)
                {
                    this.Player = null;
                }
                old.Destroy();
            }

            ClientEntity e = new ClientEntity(typeName, entityID, isPlayer, x, y, z, yaw, attrs);
            this.entities[entityID] = e;

            if (e.IsPlayer)
            {
                if (this.Player != null)
                {
                    Logger.Warn("EntityManager", "Replacing Existing Player: " + this.Player);
                }
                this.Player = e; 
            }
            return e; 
        }

        private void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - EntityManager - " + msg, args));
        }
    }


}
