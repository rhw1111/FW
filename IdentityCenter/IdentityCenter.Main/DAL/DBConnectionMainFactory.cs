using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.Collections.Hash.DAL;
using MSLibrary.Configuration;
using MSLibrary.Configuration.DAL;
using MSLibrary.Context.DAL;
using MSLibrary.Transaction;
using IdentityCenter.Main.Configuration;

namespace IdentityCenter.Main.DAL
{
    [Injection(InterfaceType = typeof(IDBConnectionMainFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(ISystemConfigurationConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IHashConnectionFactory), Scope = InjectionScope.Singleton)]
    [Injection(InterfaceType = typeof(IContextConnectionFactory), Scope = InjectionScope.Singleton)]
    public class DBConnectionMainFactory : IDBConnectionMainFactory
    {
        private readonly ISystemConfigurationService _systemConfigurationService;

        public DBConnectionMainFactory(ISystemConfigurationService systemConfigurationService)
        {
            _systemConfigurationService = systemConfigurationService;
        }
        public string CreateAllForContext()
        {
            return CreateAllForIdentityConfiguration();
        }

        public string CreateAllForHash()
        {
            return CreateAllForIdentityConfiguration();
        }

        public string CreateAllForIdentityConfiguration()
        {
            return _systemConfigurationService.GetConnectionString("mainconfigall");
        }

        public string CreateAllForIdentityEntity()
        {
            return _systemConfigurationService.GetConnectionString("mainentityall");
        }

        public string CreateAllForIdentityTemporary()
        {
            return _systemConfigurationService.GetConnectionString("maintempall");
        }

        public string CreateAllForSystemConfiguration()
        {
            return CreateAllForIdentityConfiguration();
        }

        public string CreateReadForContext()
        {
            return CreateReadForIdentityConfiguration();
        }

        public string CreateReadForHash()
        {
            return CreateReadForIdentityConfiguration();
        }

        public string CreateReadForIdentityConfiguration()
        {
            if (DBAllScope.IsAll())
            {
                return CreateAllForIdentityConfiguration();
            }
            return _systemConfigurationService.GetConnectionString("mainconfigread");
        }

        public string CreateReadForIdentityEntity()
        {
            if (DBAllScope.IsAll())
            {
                return CreateAllForIdentityEntity();
            }
            return _systemConfigurationService.GetConnectionString("mainentityread");
        }

        public string CreateReadForIdentityTemporary()
        {
            if (DBAllScope.IsAll())
            {
                return CreateAllForIdentityTemporary();
            }
            return _systemConfigurationService.GetConnectionString("maintempread");
        }

        public string CreateReadForSystemConfiguration()
        {
            return CreateReadForIdentityConfiguration();
        }
    }
}
