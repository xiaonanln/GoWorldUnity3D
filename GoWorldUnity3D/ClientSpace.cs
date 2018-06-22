using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoWorld
{
    internal class ClientSpace: ClientEntity
    {
        internal ClientSpace(string typeName, string entityID, bool isPlayer, float x, float y, float z, float yaw, Hashtable attrs)
            : base(typeName, entityID, isPlayer, x, y, z, yaw, attrs)
        {
            
        }
    }
}
