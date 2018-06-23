using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Player : GoWorld.ClientEntity
    {
        protected override void OnBecomeClientOwner()
        {
        }

        protected override void OnCreated()
        {
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnEnterSpace()
        {
            GoWorld.Logger.Info(this.ToString(), "Enter Space ... Action = {0}", this.Attrs.GetStr("action"));
        }

        protected override void OnLeaveSpace()
        {
            GoWorld.Logger.Info(this.ToString(), "Leave Space ...");
        }

        public void OnAttrChange_action()
        {
            GoWorld.Logger.Warn(this.ToString(), "Action Changed To: {0}", this.Attrs.GetStr("action"));
        }

        public void OnAttrChange_hp()
        {
            GoWorld.Logger.Warn(this.ToString(), "HP Changed To: {0}", this.Attrs.GetInt("hp"));
        }
    }
}
