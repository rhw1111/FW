using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ChannelCredentialsGeneratorServices
{
    [Injection(InterfaceType = typeof(ChannelCredentialsGeneratorServiceForInsecureFactory), Scope = InjectionScope.Singleton)]
    public class ChannelCredentialsGeneratorServiceForInsecureFactory:IFactory<IChannelCredentialsGeneratorService>
    {
        private ChannelCredentialsGeneratorServiceForInsecure _channelCredentialsGeneratorServiceForInsecure;

        public ChannelCredentialsGeneratorServiceForInsecureFactory(ChannelCredentialsGeneratorServiceForInsecure channelCredentialsGeneratorServiceForInsecure)
        {
            _channelCredentialsGeneratorServiceForInsecure = channelCredentialsGeneratorServiceForInsecure;
        }
        public IChannelCredentialsGeneratorService Create()
        {
            return _channelCredentialsGeneratorServiceForInsecure;
        }
    }
}
