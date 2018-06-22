using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GoWorld
{
    public abstract class ClientEntity
    {
        public object Data { get; set; } // Data for custom use
        public string ID { get; internal set; }
        public string TypeName { get
            {
                return this.entityType.Name;
            }
        }
        private Type entityType;

        public bool IsSpace { get
            {
                return this.entityType == typeof(ClientSpace);
            }
        }
        public bool IsClientOwner { get; internal set; }
        public float x;
        public float y;
        public float z;
        public float yaw;
        public Hashtable attrs;

        public bool IsDestroyed { get; private set; }
        public Vector3 Position { get; internal set; }
        public float Yaw { get; private set; }

        private void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - "+this+" - " + msg, args));
        }

        public override string ToString()
        {
            return this.TypeName + "<" + this.ID + "|" + (this.IsDestroyed?"D":"A") + (this.IsClientOwner?"P":"") + ">";
        }

        public void CallServer(string method, params object[] args)
        {
            GameClient.Instance.CallServer(this.ID, method, args);
        }

        public void Destroy()
        {
            if (this.IsDestroyed)
            {
                return;
            }

            try
            {
                this.OnDestroy();
            } catch (Exception e)
            {
                Logger.Error(this.ToString(), e.ToString());
            }

            EntityManager.Instance.delEntity(this);
            this.IsDestroyed = true;
            this.attrs = null;
        }

        internal void init(Type entityType, string entityID, bool isClientOwner, float x, float y, float z, float yaw, Hashtable attrs)
        {
            this.entityType = entityType;
            this.ID = entityID;
            this.IsClientOwner = isClientOwner;
            this.Position = new Vector3(x, y, z);
            this.Yaw = yaw;
            this.attrs = attrs;

            try
            {
                this.OnCreated();
            } catch (Exception e)
            {
                Logger.Error(this.ToString(), e.ToString());
            }
        }

        internal void leaveSpace()
        {
            try
            {
                this.OnLeaveSpace();
            }
            catch (Exception e)
            {
                Logger.Error(this.ToString(), e.ToString());
            }
        }

        internal void enterSpace()
        {
            try
            {
                this.OnEnterSpace();
            }
            catch (Exception e)
            {
                Logger.Error(this.ToString(), e.ToString());
            }
        }

        internal void becomeClientOwner()
        {
            try
            {
                this.OnBecomeClientOwner();
            }
            catch(Exception e)
            {
                Logger.Error(this.ToString(), e.ToString());
            }
        }

        protected abstract void OnCreated();
        protected abstract void OnBecomeClientOwner();
        protected abstract void OnEnterSpace();
        protected abstract void OnLeaveSpace();
        protected abstract void OnDestroy();

    }
}
