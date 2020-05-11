using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.Configuration.DAL;

namespace MSLibrary.Configuration
{
    [Injection(InterfaceType = typeof(ISystemConfigurationRepository), Scope = InjectionScope.Singleton)]
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private ISystemConfigurationStore _systemConfigurationStore;
        public SystemConfigurationRepository(ISystemConfigurationStore systemConfigurationStore)
        {
            _systemConfigurationStore = systemConfigurationStore;
        }
        public SystemConfiguration QueryByName(string name)
        {
            return _systemConfigurationStore.QueryByName(name);
        }
    }
}
