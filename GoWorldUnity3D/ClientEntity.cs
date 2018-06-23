using System;
using System.Diagnostics;
using UnityEngine;

namespace GoWorldUnity3D
{
    public abstract class ClientEntity
    {
        public GameObject GameObject { get {
                return this.gameObject;
            } set {
                if (this.gameObject != value)
                {
                    this.gameObject = value;
                    this.onGameObjectChanged();
                }
            }
        }

        private GameObject gameObject;
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
        public MapAttr Attrs;

        public bool IsDestroyed { get; private set; }
        public Vector3 Position { get; internal set; }
        public float Yaw { get; private set; }

        private void debug(string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - "+this+" - " + msg, args));
        }

        void Test()
        {
            UnityEngine.GameObject go;
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

        internal void init(Type entityType, string entityID, bool isClientOwner, float x, float y, float z, float yaw, MapAttr attrs)
        {
            this.entityType = entityType;
            this.ID = entityID;
            this.IsClientOwner = isClientOwner;
            this.Position = new Vector3(x, y, z);
            this.Yaw = yaw;
            this.Attrs = attrs;
        }

        internal void update()
        {
            this.Update();
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

        private void onGameObjectChanged()
        {
            throw new NotImplementedException();
        }

        protected abstract void OnCreated();
        protected abstract void OnBecomeClientOwner();
        protected abstract void OnEnterSpace();
        protected abstract void OnLeaveSpace();
        protected abstract void OnDestroy();
        protected abstract void Update();

        internal void OnSyncEntityInfo(float x, float y, float z, float yaw)
        {
            this.Position = new Vector3(x, y, z);
            this.Yaw = yaw;
        }

        internal void OnMapAttrChange(ListAttr path, string key, object val)
        {
            MapAttr t = this.getAttrByPath(path) as MapAttr;
            t.put(key, val);
            string rootkey = path != null && path.Count > 0 ? (string)path.get(0) : key;
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnMapAttrDel(ListAttr path, string key)
        {
            MapAttr t = this.getAttrByPath(path) as MapAttr;
            if (t.ContainsKey(key))
            {
                t.Remove(key);
            }
            string rootkey = path != null && path.Count > 0 ? (string)path.get(0) : key;
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnMapAttrClear(ListAttr path)
        {
            System.Diagnostics.Debug.Assert(path != null && path.Count > 0);
            MapAttr t = this.getAttrByPath(path) as MapAttr;
            t.Clear();
            string rootkey = (string)path.get(0);
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }
        
        internal void OnListAttrAppend(ListAttr path, object val)
        {
            ListAttr l = getAttrByPath(path) as ListAttr;
            l.append(val);
            string rootkey = (string)path.get(0);
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnListAttrPop(ListAttr path)
        {
            ListAttr l = getAttrByPath(path) as ListAttr;
            l.pop(l.Count - 1);
            string rootkey = (string)path.get(0);
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal void OnListAttrChange(ListAttr path, int index, object val)
        {
            ListAttr l = getAttrByPath(path) as ListAttr;
            l.set(index, val);
            string rootkey = (string)path.get(0);
            System.Reflection.MethodInfo callback = this.GetType().GetMethod("OnAttrChange_" + rootkey);
            if (callback != null)
            {
                callback.Invoke(this, new object[0]);
            }
        }

        internal object getAttrByPath(ListAttr path)
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
                    attr = (attr as MapAttr).get((string)key);
                }
                else
                {
                    attr = (attr as ListAttr).get((int)key);
                }
            }

            Logger.Debug(this.ToString(), "Get Attr By Path: {0} = {1}", path.ToString(), attr);
            return attr;
        }
    }
}
