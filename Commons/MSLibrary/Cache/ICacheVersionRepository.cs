using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Cache
{
    public interface ICacheVersionRepository
    {
        Task<QueryResult<CacheVersion>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<CacheVersion?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<CacheVersion?> QueryByID(Guid id, CancellationToken cancellationToken = default);
    }
}
