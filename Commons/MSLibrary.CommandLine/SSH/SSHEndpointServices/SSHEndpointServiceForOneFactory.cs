using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.CommandLine.SSH.SSHEndpointServices
{
    [Injection(InterfaceType = typeof(SSHEndpointServiceForOneFactory), Scope = InjectionScope.Singleton)]
    public class SSHEndpointServiceForOneFactory : IFactory<ISSHEndpointService>
    {
        private SSHEndpointServiceForOne _sshEndpointServiceForOne;

        public SSHEndpointServiceForOneFactory(SSHEndpointServiceForOne sshEndpointServiceForOne)
        {
            _sshEndpointServiceForOne = sshEndpointServiceForOne;
        }

        public ISSHEndpointService Create()
        {
            return _sshEndpointServiceForOne;
        }
    }
}
