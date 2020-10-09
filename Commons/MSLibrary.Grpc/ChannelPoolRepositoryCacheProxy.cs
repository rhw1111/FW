using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Grpc
{
    [Injection(InterfaceType = typeof(IChannelPoolRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ChannelPoolRepositoryCacheProxy : IChannelPoolRepositoryCacheProxy
    {
        private readonly IChannelPoolRepository _channelPoolRepository;

        public ChannelPoolRepositoryCacheProxy(IChannelPoolRepository channelPoolRepository)
        {
            _channelPoolRepository = channelPoolRepository;
        }
        public async Task<ChannelPool> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _channelPoolRepository.QueryByName(name, cancellationToken);
        }
    }
}
