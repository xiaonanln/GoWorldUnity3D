﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GoWorldUnity3D
{
    public class ClientSpace : ClientEntity
    {
        protected override void OnCreated()
        {
        }

        protected override void OnBecomeClientOwner()
        {
            throw new NotImplementedException();
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnEnterSpace()
        {
            throw new NotImplementedException();
        }

        protected override void OnLeaveSpace()
        {
            throw new NotImplementedException();
        }

        public static new GameObject CreateGameObject(MapAttr attrs)
        {
            return new GameObject("ClientSpace", typeof(ClientSpace));
        }

        protected override void Tick()
        {
        }
    }
}
