using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Monster : GoWorld.ClientEntity
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
            GoWorld.Logger.Info(this.ToString(), "Enter Space ...");
        }

        protected override void OnLeaveSpace()
        {
            GoWorld.Logger.Info(this.ToString(), "Leave Space ...");
        }

        public void DisplayAttack(string victimID)
        {
            GoWorld.Logger.Info(this.ToString(), "Attacking Player {0}", victimID);
        }
    }
}
