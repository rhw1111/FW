using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Configuration;
using MSLibrary.DI;

namespace MSLibrary.Security
{
    /// <summary>
    /// 实现安全服务所使用的配置服务
    /// </summary>
    [Injection(InterfaceType =typeof(ISecurityConfigurationService),Scope = InjectionScope.Singleton)]
    public class SecurityConfigurationService : ISecurityConfigurationService
    {
        public string GetEncryptIV()
        {
            var configuration= ConfigurationContainer.GetBySection<SecurityConfiguration>("EncryptInfo");
            return configuration.EncryptIV;
        }

        public string GetEncryptKey()
        {
            var configuration = ConfigurationContainer.GetBySection<SecurityConfiguration>("EncryptInfo");
            return configuration.EncryptKey;
        }
    }


    public class SecurityConfiguration
    {
        public string EncryptKey { get; set; }

        public string EncryptIV { get; set; }
    }
}
