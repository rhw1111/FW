using MSLibrary.Collections.Hash.HashDataMigrateServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Grpc
{
    public interface IChannelPoolRepository
    {
        Task<ChannelPool> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
