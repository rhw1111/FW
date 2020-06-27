using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Configuration;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Configuration
{
    [Injection(InterfaceType = typeof(ISystemConfigurationService), Scope = InjectionScope.Singleton)]
    public class SystemConfigurationService : ISystemConfigurationService
    {
        //private readonly ISystemConfigurationRepositoryCacheProxy _systemConfigurationRepositoryCacheProxy;

        public SystemConfigurationService()
        {
            //_systemConfigurationRepositoryCacheProxy = systemConfigurationRepositoryCacheProxy;
        }

        public string[] GetApplicationCrosOrigin(CancellationToken cancellationToken = default)
        {
            var systemConfigurationRepositoryCacheProxy = DIContainerContainer.Get<ISystemConfigurationRepositoryCacheProxy>();
            var appConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            var configurationName = string.Format(SystemConfigurationItemNames.ApplicationCrosOrigin, appConfiguration.ApplicationName);
            var appCrosOrigin = systemConfigurationRepositoryCacheProxy.QueryByName(configurationName);
            if (appCrosOrigin == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSystemConfigurationByName,
                    DefaultFormatting = "找不到名称为{0}的系统配置",
                    ReplaceParameters = new List<object>() { configurationName }
                };

                throw new UtilityException((int)Errors.NotFoundSystemConfigurationByName, fragment, 1, 0);
            }

            return appCrosOrigin.GetConfigurationValue<string[]>();

        }

        public async Task<string[]> GetApplicationCrosOriginAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(GetApplicationCrosOrigin(cancellationToken));
        }

        public string GetCaseServiceBaseAddress(CancellationToken cancellationToken = default)
        {
            var systemConfigurationRepositoryCacheProxy = DIContainerContainer.Get<ISystemConfigurationRepositoryCacheProxy>();
            var appConfiguration = systemConfigurationRepositoryCacheProxy.QueryByName(SystemConfigurationItemNames.CaseServiceBaseAddress);
            if (appConfiguration == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSystemConfigurationByName,
                    DefaultFormatting = "找不到名称为{0}的系统配置",
                    ReplaceParameters = new List<object>() { SystemConfigurationItemNames.CaseServiceBaseAddress }
                };

                throw new UtilityException((int)Errors.NotFoundSystemConfigurationByName, fragment, 1, 0);
            }

            return appConfiguration.GetConfigurationValue<string>();
        }

        public async Task<string> GetCaseServiceBaseAddressAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(GetCaseServiceBaseAddress(cancellationToken));
        }

        public string GetConnectionString(string name, CancellationToken cancellationToken)
        {
            var appConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            return appConfiguration.Connections[name];
        }

        public async Task<string> GetConnectionStringAsync(string name, CancellationToken cancellationToken)
        {
            return await Task.FromResult(GetConnectionString(name, cancellationToken));
        }

        public Guid GetDefaultUserID(CancellationToken cancellationToken = default)
        {

            var systemConfigurationRepositoryCacheProxy = DIContainerContainer.Get<ISystemConfigurationRepositoryCacheProxy>();
            var appConfiguration=systemConfigurationRepositoryCacheProxy.QueryByName(SystemConfigurationItemNames.DefaultUserID);
            if (appConfiguration==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSystemConfigurationByName,
                    DefaultFormatting = "找不到名称为{0}的系统配置",
                    ReplaceParameters = new List<object>() { SystemConfigurationItemNames.DefaultUserID }
                };

                throw new UtilityException((int)Errors.NotFoundSystemConfigurationByName, fragment, 1, 0);
            }

            return appConfiguration.GetConfigurationValue<Guid>();
        }

        public async Task<Guid> GetDefaultUserIDAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(GetDefaultUserID(cancellationToken));
        }

        public string GetHostApplicationName(CancellationToken cancellationToken = default)
        {
            var appConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            return appConfiguration.ApplicationName;
        }

        public async Task<string> GetHostApplicationNameAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(GetHostApplicationName(cancellationToken));
        }
    }
}
