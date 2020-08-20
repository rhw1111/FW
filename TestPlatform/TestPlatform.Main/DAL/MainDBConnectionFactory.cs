using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.Configuration.DAL;
using MSLibrary.Logger.DAL;
using MSLibrary.Transaction;
using MSLibrary.Context.DAL;
using MSLibrary.StreamingDB.DAL;
using FW.TestPlatform.Main.Configuration;
using MSLibrary.CommandLine.SSH.DAL;
using MSLibrary.Schedule.DAL;
using MSLibrary.Collections.DAL;

namespace FW.TestPlatform.Main.DAL
{ 
    [Injection(InterfaceType = typeof(ICommonLogConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(ISystemConfigurationConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IContextConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IMainDBConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IStreamingDBConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(ICommandLineConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IScheduleConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(ICollectionConnectionFactory), Scope = InjectionScope.Singleton)]
    public class MainDBConnectionFactory : IMainDBConnectionFactory
    {
        private readonly ISystemConfigurationService _systemConfigurationService;

        public MainDBConnectionFactory(ISystemConfigurationService systemConfigurationService)
        {
            _systemConfigurationService = systemConfigurationService;
        }

        public string CreateAllForCollection()
        {
            return CreateAllForMain();
        }

        public string CreateAllForCommandLine()
        {
            return CreateAllForLocalCommonLog();
        }

        public string CreateAllForContext()
        {
            return CreateAllForLocalCommonLog();
        }

        public string CreateAllForLocalCommonLog()
        {
            return _systemConfigurationService.GetConnectionString("configurationall");
        }

        public string CreateAllForMain()
        {
            return _systemConfigurationService.GetConnectionString("mainall");
        }

        public string CreateAllForSchedule()
        {
            return CreateAllForLocalCommonLog();
        }

        public string CreateAllForStreamingDB()
        {
            return CreateAllForLocalCommonLog();
        }

        public string CreateAllForSystemConfiguration()
        {
            return CreateAllForLocalCommonLog();
        }

        public string CreateReadForCollection()
        {
            return CreateReadForMain();
        }

        public string CreateReadForCommandLine()
        {
            return CreateReadForLocalCommonLog();
        }

        public string CreateReadForContext()
        {
            return CreateReadForLocalCommonLog();
        }

        public string CreateReadForLocalCommonLog()
        {
            if (DBAllScope.IsAll())
            {
                return CreateAllForLocalCommonLog();
            }
            return _systemConfigurationService.GetConnectionString("confifurationread");
        }

        public string CreateReadForMain()
        {
            if (DBAllScope.IsAll())
            {
                return CreateAllForLocalCommonLog();
            }
            return _systemConfigurationService.GetConnectionString("mainread");
        }

        public string CreateReadForSchedule()
        {
            return CreateReadForLocalCommonLog();
        }

        public string CreateReadForStreamingDB()
        {
            return CreateReadForLocalCommonLog();
        }

        public string CreateReadForSystemConfiguration()
        {
            return CreateReadForLocalCommonLog();
        }
    }
}
