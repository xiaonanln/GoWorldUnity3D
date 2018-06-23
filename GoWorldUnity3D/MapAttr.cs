﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GoWorld
{
    public class MapAttr
    {
        private Dictionary<string, object> dict = new Dictionary<string, object>();

        public string GetStr(string key)
        {
            object val = this.get(key);
            return val != null ? val as string : "";
        }

        public Int64 GetInt(string key)
        {
            object val = this.get(key);
            return (Int64)val;
        }

        public bool GetBool(string key)
        {
            object val = this.get(key);
            return (bool)(val);
        }

        public MapAttr GetMapAttr(string key)
        {
            object val = this.get(key);
            return val != null ? val as MapAttr : new MapAttr();
        }

        public ListAttr GetListAttr(string key)
        {
            object val = this.get(key);
            return val != null ? val as ListAttr : new ListAttr();
        }

        internal object get(string key)
        {
            try
            {
                return this.dict[key];
            } catch (KeyNotFoundException)
            {
                return null;
            }
        }

        internal void put(string key, object val)
        {
            DataPacker.ValidateDataType(val);
            this.dict[key] = val;
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return this.dict.GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return this.dict.ContainsKey(key);
        }

        public void Remove(string key)
        {
            try
            {
                this.dict.Remove(key);
            } catch (KeyNotFoundException )
            {

            }
        }

        public void Clear()
        {
            this.dict.Clear();
        }
    }
}