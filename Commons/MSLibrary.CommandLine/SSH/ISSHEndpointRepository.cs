using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.CommandLine.SSH
{
    public interface ISSHEndpointRepository
    {
        Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<SSHEndpoint?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<QueryResult<SSHEndpoint>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default);
    }
}
