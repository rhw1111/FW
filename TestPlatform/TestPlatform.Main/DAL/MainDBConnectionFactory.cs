using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.Configuration.DAL;
using MSLibrary.Logger.DAL;
using MSLibrary.Transaction;
using MSLibrary.Context.DAL;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.DAL
{ 
    [Injection(InterfaceType = typeof(ICommonLogConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(ISystemConfigurationConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IContextConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IMainDBConnectionFactory), Scope = InjectionScope.Singleton)]
    public class MainDBConnectionFactory : IMainDBConnectionFactory
    {
        private readonly ISystemConfigurationService _systemConfigurationService;

        public MainDBConnectionFactory(ISystemConfigurationService systemConfigurationService)
        {
            _systemConfigurationService = systemConfigurationService;
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

        public string CreateAllForSystemConfiguration()
        {
            return CreateAllForLocalCommonLog();
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

        public string CreateReadForSystemConfiguration()
        {
            return CreateReadForLocalCommonLog();
        }
    }
}
