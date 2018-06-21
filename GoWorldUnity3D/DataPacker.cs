using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GoWorld
{
    class DataPacker
    {
        internal static object UnpackData(byte[] data)
        {
            MsgPack.MessagePackObject mpobj = MsgPack.Unpacking.UnpackObject(data).Value;
            return convertFromMsgPackObject(mpobj);
        }

        internal static byte[] PackData(object v)
        {
            MsgPack.MessagePackObject mpobj = convertToMsgPackObject(v);
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            MsgPack.Packer packer = MsgPack.Packer.Create(stream);
            mpobj.PackToMessage(packer, null);
            stream.Flush();
            return stream.GetBuffer();
        }

        static MsgPack.MessagePackObject convertToMsgPackObject(object v)
        {
            Type t = v.GetType();
            if (t.Equals(typeof(Hashtable)))
            {
                Hashtable ht = v as Hashtable;
                IDictionaryEnumerator e = ht.GetEnumerator();
                MsgPack.MessagePackObjectDictionary d = new MsgPack.MessagePackObjectDictionary();
                while (e.MoveNext())
                {
                    d.Add(new MsgPack.MessagePackObject(e.Key as string), convertToMsgPackObject(e.Value));
                }
                return new MsgPack.MessagePackObject(d);
            }
            else if (t.Equals(typeof(ArrayList)))
            {
                ArrayList al = v as ArrayList;
                IEnumerator e = al.GetEnumerator();
                System.Collections.Generic.IList<MsgPack.MessagePackObject> l = new System.Collections.Generic.List<MsgPack.MessagePackObject>();
                while (e.MoveNext())
                {
                    l.Add(convertToMsgPackObject(e.Current));
                }
                return new MsgPack.MessagePackObject(l);
            }
            else if (t.Equals(typeof(bool)))
            {
                return new MsgPack.MessagePackObject((bool)v);
            }
            else if (t.Equals(typeof(string)))
            {
                return new MsgPack.MessagePackObject((string)v);
            }
            else
            {
                Debug.Assert(false, "Unknwon type: " + t.Name);
                return new MsgPack.MessagePackObject();
            }
        }

        static object convertFromMsgPackObject(MsgPack.MessagePackObject mpobj)
        {
            if (mpobj.IsDictionary)
            {
                return convertFromMsgPackObjectDictionary(mpobj.AsDictionary());
            }
            if (mpobj.IsList)
            {
                return convertFromMsgPackObjectList(mpobj.AsList());
            }
            return mpobj.ToObject();
        }

        static Hashtable convertFromMsgPackObjectDictionary(MsgPack.MessagePackObjectDictionary mpobj)
        {
            Hashtable ht = new Hashtable();
            MsgPack.MessagePackObjectDictionary.Enumerator e = mpobj.GetEnumerator();
            while (e.MoveNext())
            {
                MsgPack.MessagePackObject key = e.Current.Key;
                MsgPack.MessagePackObject val = e.Current.Value;
                ht.Add(key.AsString(), convertFromMsgPackObject(val));
            }
            return ht;
        }

        static ArrayList convertFromMsgPackObjectList(IList<MsgPack.MessagePackObject> mpobj)
        {
            ArrayList list = new ArrayList(mpobj.Count);
            IEnumerator<MsgPack.MessagePackObject> e = mpobj.GetEnumerator();
            while (e.MoveNext())
            {
                list.Add(convertFromMsgPackObject(e.Current));
            }
            return list;
        }

    }
}
