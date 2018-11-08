﻿using System;
using System.Reflection;
using Iot.Common.ClassLogger;
using IotDataServer.HttpServer;

namespace IotDataServer
{
    public class DataServer
    {
        private static ClassLogger Logger = ClassLogManager.GetCurrentClassLogger();
        private readonly DataServerSetting _setting;
        private WebServer _webServer;
        private DataGetterManager _dataGetter = null;
        private DataManager _dataManager = null;

        public DataServer(DataServerSetting setting)
        {
            _setting = setting;
            Init();
        }

        public void Init()
        {
            _dataManager = DataManager.Instance;
            _dataManager.Initialize(_setting);
            _webServer = new WebServer(_setting.ServicePort);
        }


        public bool Start()
        {
            bool res = false;
            _dataGetter = new DataGetterManager(_setting.GetterSettings, Assembly.GetAssembly(this.GetType()), _dataManager);
            try
            {
                _dataGetter.Start();
                _webServer.Start();
                res = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Cannot start!");
            }

            return res;
        }

        public void Stop()
        {
            _webServer.Stop();
            _dataGetter.Stop();
        }
    }
}