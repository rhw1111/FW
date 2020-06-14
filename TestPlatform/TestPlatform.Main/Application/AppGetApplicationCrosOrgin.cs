using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MSLibrary.DI;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppGetApplicationCrosOrgin), Scope = InjectionScope.Singleton)]
    public class AppGetApplicationCrosOrgin : IAppGetApplicationCrosOrgin
    {
        private readonly ISystemConfigurationService _systemConfigurationService;

        public AppGetApplicationCrosOrgin(ISystemConfigurationService systemConfigurationService)
        {
            _systemConfigurationService = systemConfigurationService;
        }
        public string[] Do(CancellationToken cancellationToken = default)
        {
            return _systemConfigurationService.GetApplicationCrosOrigin(cancellationToken);
        }
    }
}
