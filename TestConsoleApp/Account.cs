﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Account : GoWorld.ClientEntity
    {

        const string USERNAME = "unity3dlib2";
        const string PASSWORD = "unity3dlib2";

        protected override void OnCreated()
        {
            GoWorld.Logger.Info(this.ToString(), "OnCreated ...");
        }

        protected override void OnBecomeClientOwner()
        {
            GoWorld.Logger.Info(this.ToString(), "OnBecomeClientOwner ...");

            // Account created, logging 
            this.CallServer("Register", USERNAME, PASSWORD);
        }

        protected override void OnDestroy()
        {
            GoWorld.Logger.Info(this.ToString(), "OnDestroy ...");
        }

        public void ShowError(string err)
        {
            GoWorld.Logger.Error("ERROR", err);

            if (err.Contains("aready exists"))
            {
                this.onRegisterSuccessfully();
            }
        }
        public void ShowInfo(string info)
        {
            GoWorld.Logger.Info("INFO", info);
            if (info.Contains("Registered Successfully"))
            {
                this.onRegisterSuccessfully();
            }
        }

        private void onRegisterSuccessfully()
        {
            // register ok, start login 
            this.CallServer("Login", USERNAME, PASSWORD);
        }

        protected override void OnEnterSpace()
        {
            throw new NotImplementedException();
        }

        protected override void OnLeaveSpace()
        {
            throw new NotImplementedException();
        }
    }
}
