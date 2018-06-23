using System;
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

        public void Put(string key, object val)
        {
            DataPacker.ValidateDataType(val);
            this.dict[key] = val;
        }

        internal IDictionaryEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
