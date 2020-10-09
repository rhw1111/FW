using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.CommandLine.SSH.SSHEndpointServices
{
    [Injection(InterfaceType = typeof(SSHEndpointServiceForSyncFactory), Scope = InjectionScope.Singleton)]
    public class SSHEndpointServiceForSyncFactory : IFactory<ISSHEndpointService>
    {
        private SSHEndpointServiceForSync _sshEndpointServiceForSync;

        public SSHEndpointServiceForSyncFactory(SSHEndpointServiceForSync sshEndpointServiceForSync)
        {
            _sshEndpointServiceForSync = sshEndpointServiceForSync;
        }

        public ISSHEndpointService Create()
        {
            return _sshEndpointServiceForSync;
        }
    }
}
