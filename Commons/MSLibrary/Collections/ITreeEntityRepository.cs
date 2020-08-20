using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Collections
{
    public interface ITreeEntityRepository
    {
        Task<QueryResult<TreeEntity>> Query(string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<QueryResult<TreeEntity>> QueryRoot(string? matchName, int? type, int page, int pageSize, CancellationToken cancellationToken = default);

        Task<TreeEntity?> QueryByID(Guid id, CancellationToken cancellationToken = default);
    }
}
