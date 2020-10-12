using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.CommandLine.SSH.SSHEndpointServices
{
    [Injection(InterfaceType = typeof(SSHEndpointServiceForOnceFactory), Scope = InjectionScope.Singleton)]
    public class SSHEndpointServiceForOnceFactory : IFactory<ISSHEndpointService>
    {
        private SSHEndpointServiceForOnce _sshEndpointServiceForOnce;

        public SSHEndpointServiceForOnceFactory(SSHEndpointServiceForOnce sshEndpointServiceForOnce)
        {
            _sshEndpointServiceForOnce = sshEndpointServiceForOnce;
        }

        public ISSHEndpointService Create()
        {
            return _sshEndpointServiceForOnce;
        }
    }
}
