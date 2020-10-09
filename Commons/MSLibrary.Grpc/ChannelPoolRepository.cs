using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Grpc
{
    [Injection(InterfaceType = typeof(IChannelPoolRepository), Scope = InjectionScope.Singleton)]
    public class ChannelPoolRepository : IChannelPoolRepository
    {
        public static IDictionary<string, ChannelPool> ChannelPools { get; } = new Dictionary<string, ChannelPool>();

        public async Task<ChannelPool> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(ChannelPools[name]);
        }
    }
}
