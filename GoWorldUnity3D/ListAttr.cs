using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoWorld
{
    public class ListAttr
    {
        private ArrayList list = new ArrayList();
        public int Count;

        internal void append(object val)
        {
            DataPacker.ValidateDataType(val);
            this.list.Add(val);
        }

        internal void pop(int index)
        {
            this.list.RemoveAt(index);
        }

        internal void set(int index, object val)
        {
            DataPacker.ValidateDataType(val);
            this.list[index] = val;
        }

        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        internal object get(int index)
        {
            return this.list[index];
        }
    }
}
