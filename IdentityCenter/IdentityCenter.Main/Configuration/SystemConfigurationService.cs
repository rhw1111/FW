using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Configuration
{
    [Injection(InterfaceType = typeof(ISystemConfigurationService), Scope = InjectionScope.Singleton)]
    public class SystemConfigurationService : ISystemConfigurationService
    {
        public string GetConnectionString(string name, CancellationToken cancellationToken = default)
        {
            var appConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            return appConfiguration.Connections[name];
        }

        public async Task<string> GetIdentityClientApplicationName(CancellationToken cancellationToken = default)
        {
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            return await Task.FromResult(coreConfiguration.ApplicationName);
        }

        public async Task<string> GetIdentityHostApplicationName(CancellationToken cancellationToken = default)
        {
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            return await Task.FromResult(coreConfiguration.ApplicationName);
        }

    }
}
