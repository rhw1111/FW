using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ServerCredentialsGeneratorServices
{
    [Injection(InterfaceType = typeof(ServerCredentialsGeneratorServiceForSSLFactory), Scope = InjectionScope.Singleton)]
    public class ServerCredentialsGeneratorServiceForSSLFactory:IFactory<IServerCredentialsGeneratorService>
    {
        private ServerCredentialsGeneratorServiceForSSL _serverCredentialsGeneratorServiceForSSL;

        public ServerCredentialsGeneratorServiceForSSLFactory(ServerCredentialsGeneratorServiceForSSL serverCredentialsGeneratorServiceForSSL)
        {
            _serverCredentialsGeneratorServiceForSSL = serverCredentialsGeneratorServiceForSSL;
        }
        public IServerCredentialsGeneratorService Create()
        {
            return _serverCredentialsGeneratorServiceForSSL;
        }
    }
}
