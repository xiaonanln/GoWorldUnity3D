using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoWorld
{
    public class ClientEntity
    {
        public object Data { get; set; } // Data for custom use
        public readonly string ID;
        public readonly string TypeName;
        public readonly bool IsPlayer;
        public float x;
        public float y;
        public float z;
        public float yaw;
        public Hashtable attrs;

        public bool IsDestroyed { get; private set; }

        internal ClientEntity(string typeName, string entityID, bool isPlayer, float x, float y, float z, float yaw, Hashtable attrs)
        {
            this.TypeName = typeName;
            this.ID = entityID;
            this.IsPlayer = isPlayer;
            this.x = x;
            this.y = y;
            this.z = z;
            this.yaw = yaw;
            this.attrs = attrs;
        }

        private void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - "+this+" - " + msg, args));
        }

        public override string ToString()
        {
            return this.TypeName + "<" + this.ID + "|" + (this.IsDestroyed?"D":"A") + (this.IsPlayer?"P":"") + ">";
        }

        public void Destroy()
        {
            if (this.IsDestroyed)
            {
                return;
            }

            this.IsDestroyed = true;
            this.attrs = null;
        }
    }
}
