using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ChannelCredentialsGeneratorServices
{
    [Injection(InterfaceType = typeof(ChannelCredentialsGeneratorServiceForSSLFactory), Scope = InjectionScope.Singleton)]
    public class ChannelCredentialsGeneratorServiceForSSLFactory:IFactory<IChannelCredentialsGeneratorService>
    {
        private ChannelCredentialsGeneratorServiceForSSL _channelCredentialsGeneratorServiceForSSL;

        public ChannelCredentialsGeneratorServiceForSSLFactory(ChannelCredentialsGeneratorServiceForSSL channelCredentialsGeneratorServiceForSSL)
        {
            _channelCredentialsGeneratorServiceForSSL = channelCredentialsGeneratorServiceForSSL;
        }
        public IChannelCredentialsGeneratorService Create()
        {
            return _channelCredentialsGeneratorServiceForSSL;
        }
    }
}
