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
        public Hashtable Attrs;

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

            EntityManager.Instance.delEntity(this);

            try
            {
                this.OnDestroy();
            }
            catch (Exception e)
            {
                Logger.Error(this.ToString(), e.ToString());
            }

            this.IsDestroyed = true;
            this.Attrs = null;
        }

        internal void onCreated()
        {
            try
            {
                this.OnCreated();
            }
            catch (Exception e)
            {
                Logger.Error(this.ToString(), e.ToString());
            }
        }

        internal void init(Type entityType, string entityID, bool isClientOwner, float x, float y, float z, float yaw, Hashtable attrs)
        {
            this.entityType = entityType;
            this.ID = entityID;
            this.IsClientOwner = isClientOwner;
            this.Position = new Vector3(x, y, z);
            this.Yaw = yaw;
            this.Attrs = attrs;
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

        internal void OnSyncEntityInfo(float x, float y, float z, float yaw)
        {
            this.Position = new Vector3(x, y, z);
            this.Yaw = yaw;
        }

        internal void OnMapAttrChange(ArrayList path, string key, object val)
        {
            Hashtable t = this.getAttrByPath(path) as Hashtable;
            t[key] = val;
            string rootkey = path != null && path.Count > 0 ? (string)path[0] : key;
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnMapAttrDel(ArrayList path, string key)
        {
            Hashtable t = this.getAttrByPath(path) as Hashtable;
            if (t.ContainsKey(key))
            {
                t.Remove(key);
            }
            string rootkey = path != null && path.Count > 0 ? (string)path[0] : key;
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnMapAttrClear(ArrayList path)
        {
            Debug.Assert(path != null && path.Count > 0);
            Hashtable t = this.getAttrByPath(path) as Hashtable;
            t.Clear();
            string rootkey = (string)path[0];
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }
        
        internal void OnListAttrAppend(ArrayList path, object val)
        {
            ArrayList l = getAttrByPath(path) as ArrayList;
            l.Add(val);
            string rootkey = (string)path[0];
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnListAttrPop(ArrayList path)
        {
            ArrayList l = getAttrByPath(path) as ArrayList;
            l.RemoveAt(l.Count - 1);
            string rootkey = (string)path[0];
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnListAttrChange(ArrayList path, int index, object val)
        {
            ArrayList l = getAttrByPath(path) as ArrayList;
            l[index] = val;
            string rootkey = (string)path[0];
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal object getAttrByPath(ArrayList path)
        {
            object attr = this.Attrs;

            if (path == null)
            {
                return attr;
            }

            foreach (object key in path)
            {
                if (key.GetType() == typeof(string))
                {
                    attr = (attr as Hashtable)[(string)key];
                }
                else
                {
                    attr = (attr as ArrayList)[(int)key];
                }
            }

            Logger.Debug(this.ToString(), "Get Attr By Path: {0} = {1}", path.ToString(), attr);
            return attr;
        }
    }
}
