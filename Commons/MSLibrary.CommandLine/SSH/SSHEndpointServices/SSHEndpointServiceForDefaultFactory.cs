using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.CommandLine.SSH.SSHEndpointServices
{
    [Injection(InterfaceType = typeof(SSHEndpointServiceForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class SSHEndpointServiceForDefaultFactory : IFactory<ISSHEndpointService>
    {
        private SSHEndpointServiceForDefault _sshEndpointServiceForDefault;

        public SSHEndpointServiceForDefaultFactory(SSHEndpointServiceForDefault sshEndpointServiceForDefault)
        {
            _sshEndpointServiceForDefault = sshEndpointServiceForDefault;
        }

        public ISSHEndpointService Create()
        {
            return _sshEndpointServiceForDefault;
        }
    }
}
