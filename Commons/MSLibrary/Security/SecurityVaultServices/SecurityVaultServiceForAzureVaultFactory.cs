using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.SecurityVaultServices
{
    [Injection(InterfaceType = typeof(SecurityVaultServiceForAzureVaultFactory), Scope = InjectionScope.Singleton)]
    public class SecurityVaultServiceForAzureVaultFactory : IFactory<ISecurityVaultService>
    {
        public ISecurityVaultService Create()
        {
            throw new NotImplementedException();
        }
    }
}
