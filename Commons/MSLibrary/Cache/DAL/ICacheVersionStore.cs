using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Cache.DAL
{
    public interface ICacheVersionStore
    {
        Task Add(CacheVersion version, CancellationToken cancellationToken = default);
        Task UpdateVersion(Guid id,string version, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<QueryResult<CacheVersion>> QueryByPage(string matchName,int page,int pageSize, CancellationToken cancellationToken = default);
        Task<CacheVersion?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default);
        Task<CacheVersion?> QueryByID(Guid id, CancellationToken cancellationToken = default);
    }
}
