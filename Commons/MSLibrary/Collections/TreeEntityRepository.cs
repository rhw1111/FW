using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Collections.DAL;

namespace MSLibrary.Collections
{
    [Injection(InterfaceType = typeof(ITreeEntityRepository), Scope = InjectionScope.Singleton)]
    public class TreeEntityRepository : ITreeEntityRepository
    {
        private readonly ITreeEntityStore _treeEntityStore;

        public TreeEntityRepository(ITreeEntityStore treeEntityStore)
        {
            _treeEntityStore = treeEntityStore;
        }
        public async Task<QueryResult<TreeEntity>> Query(string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _treeEntityStore.Query(matchName, type,page, pageSize, cancellationToken);
        }

        public async Task<TreeEntity> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _treeEntityStore.QueryByID(id, cancellationToken);
        }

        public async Task<QueryResult<TreeEntity>> QueryRoot(string matchName, int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _treeEntityStore.QueryChildren(null,matchName, type, page, pageSize, cancellationToken);
        }

        public async Task<QueryResult<TreeEntity>> QueryChildren(Guid parentId, string matchName, int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _treeEntityStore.QueryChildren(parentId, matchName, type, page, pageSize, cancellationToken);
        }
    }
}
