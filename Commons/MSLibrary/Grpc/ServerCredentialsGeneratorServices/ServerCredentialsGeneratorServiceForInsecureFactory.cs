using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ServerCredentialsGeneratorServices
{
    [Injection(InterfaceType = typeof(ServerCredentialsGeneratorServiceForInsecureFactory), Scope = InjectionScope.Singleton)]
    public class ServerCredentialsGeneratorServiceForInsecureFactory: IFactory<IServerCredentialsGeneratorService>
    {
        private ServerCredentialsGeneratorServiceForInsecure _serverCredentialsGeneratorServiceForInsecure;

        public ServerCredentialsGeneratorServiceForInsecureFactory(ServerCredentialsGeneratorServiceForInsecure serverCredentialsGeneratorServiceForInsecure)
        {
            _serverCredentialsGeneratorServiceForInsecure = serverCredentialsGeneratorServiceForInsecure;
        }
        public IServerCredentialsGeneratorService Create()
        {
            return _serverCredentialsGeneratorServiceForInsecure;
        }
    }
}
