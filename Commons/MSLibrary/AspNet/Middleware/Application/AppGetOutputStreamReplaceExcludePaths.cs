using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Configuration;
using System.Runtime.CompilerServices;

namespace MSLibrary.AspNet.Middleware.Application
{
    [Injection(InterfaceType = typeof(IAppGetOutputStreamReplaceExcludePaths), Scope = InjectionScope.Singleton)]
    public class AppGetOutputStreamReplaceExcludePaths : IAppGetOutputStreamReplaceExcludePaths
    {
        private ISystemConfigurationRepositoryCacheProxy _systemConfigurationRepositoryCacheProxy;

        public AppGetOutputStreamReplaceExcludePaths(ISystemConfigurationRepositoryCacheProxy systemConfigurationRepositoryCacheProxy)
        {
            _systemConfigurationRepositoryCacheProxy = systemConfigurationRepositoryCacheProxy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<List<string>> Do()
        {
            var configuration = _systemConfigurationRepositoryCacheProxy.QueryByName(SystemConfigurationNamesForAspNetMA.OutputStreamReplaceExcludePaths);
            var excludePaths = configuration.GetConfigurationValue<List<string>>();

            if (excludePaths == null)
            {
                return await Task.FromResult(new List<string>());
            }
            else
            {
                return await Task.FromResult(excludePaths);
            }
        }
    }
}
