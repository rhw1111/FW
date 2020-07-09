using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.CommandLine.SSH.DAL
{
    /// <summary>
    /// SSH终结点数据操作
    /// </summary>
    public interface ISSHEndpointStore
    {
        Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task Add(SSHEndpoint source, CancellationToken cancellationToken = default);
        Task Update(SSHEndpoint source, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<SSHEndpoint?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        //Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);

        Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default);

        Task<QueryResult<SSHEndpoint>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default);
        Task DeleteMutiple(List<Guid> ids, CancellationToken cancellationToken = default);
    }
}
