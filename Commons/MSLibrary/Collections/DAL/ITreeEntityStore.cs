using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Collections.DAL
{
    public interface ITreeEntityStore
    {
        Task Add(TreeEntity entity, CancellationToken cancellationToken = default);
        Task Update(TreeEntity entity, CancellationToken cancellationToken = default);
        Task UpdateParent(Guid id,Guid? parentID, CancellationToken cancellationToken = default);
        Task UpdateName(Guid id,string name, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<QueryResult<TreeEntity>> QueryChildren(Guid? partentID, string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<Guid?> QueryFirstChildren(Guid partentID,CancellationToken cancellationToken = default);

        Task<QueryResult<TreeEntity>> Query(string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<TreeEntity?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<TreeEntity?> QueryByName(Guid? parentId, string name, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByNameNoLock(Guid? parentId, string name, CancellationToken cancellationToken = default);
        Task<TreeEntity?> QueryWithParentByID(Guid id, CancellationToken cancellationToken = default);
    }
}
