using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Configuration;
using System.Runtime.CompilerServices;

namespace MSLibrary.AspNet.Middleware.Application
{
    [Injection(InterfaceType = typeof(IAppGetLogExcludePaths), Scope = InjectionScope.Singleton)]
    public class AppGetLogExcludePaths : IAppGetLogExcludePaths
    {
        private ISystemConfigurationRepositoryCacheProxy _systemConfigurationRepositoryCacheProxy;

        public AppGetLogExcludePaths(ISystemConfigurationRepositoryCacheProxy systemConfigurationRepositoryCacheProxy)
        {
            _systemConfigurationRepositoryCacheProxy = systemConfigurationRepositoryCacheProxy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<List<string>> Do()
        {
            var configuration= _systemConfigurationRepositoryCacheProxy.QueryByName(SystemConfigurationNamesForAspNetMA.LogExcludePaths);
            var excludePaths=configuration.GetConfigurationValue<List<string>>();

            if (excludePaths==null)
            {
                return await Task.FromResult(new List<string>());
            }
            else
            {
                return await Task.FromResult(excludePaths);
            }
        }
    }

    public static class SystemConfigurationNamesForAspNetMA
    {
        /// <summary>
        /// 排除日志记录的路径
        /// </summary>
        public const string LogExcludePaths = "LogExcludePaths";
        /// <summary>
        /// 不需要参与输出流替换的路径
        /// </summary>
        public const string OutputStreamReplaceExcludePaths = "OutputStreamReplaceExcludePaths";
    }
}
