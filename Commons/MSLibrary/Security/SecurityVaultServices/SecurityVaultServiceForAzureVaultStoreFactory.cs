using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.SecurityVaultServices
{
    [Injection(InterfaceType = typeof(SecurityVaultServiceForAzureVaultStoreFactory), Scope = InjectionScope.Singleton)]
    public class SecurityVaultServiceForAzureVaultStoreFactory : IFactory<ISecurityVaultService>
    {
        private SecurityVaultServiceForAzureVaultStore _securityVaultServiceForAzureVaultStore;

        public SecurityVaultServiceForAzureVaultStoreFactory(SecurityVaultServiceForAzureVaultStore securityVaultServiceForAzureVaultStore)
        {
            _securityVaultServiceForAzureVaultStore = securityVaultServiceForAzureVaultStore;
        }
        public ISecurityVaultService Create()
        {
            return _securityVaultServiceForAzureVaultStore;
        }
    }
}
