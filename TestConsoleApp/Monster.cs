using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Monster : GoWorldUnity3D.ClientEntity
    {
        protected override void OnBecomeClientOwner()
        {
            throw new NotImplementedException();
        }

        protected override void OnCreated()
        {
        }

        protected override void OnDestroy()
        {

        }

        protected override void OnEnterSpace()
        {
            GoWorldUnity3D.Logger.Info(this.ToString(), "Enter Space ...");
        }

        protected override void OnLeaveSpace()
        {
            GoWorldUnity3D.Logger.Info(this.ToString(), "Leave Space ...");
        }

        public void DisplayAttack(string victimID)
        {
            GoWorldUnity3D.Logger.Info(this.ToString(), "Attacking Player {0}", victimID);
        }

        public void OnAttrChange_action()
        {
            GoWorldUnity3D.Logger.Warn(this.ToString(), "Action Changed To: {0}", this.Attrs.GetStr("action"));
        }

        public void OnAttrChange_hp()
        {
            GoWorldUnity3D.Logger.Warn(this.ToString(), "HP Changed To: {0}", this.Attrs.GetInt("hp"));
        }

        protected override void Update()
        {
        }
    }
}
