using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.EntityMetadata.DAL;

namespace MSLibrary.EntityMetadata
{

    [Injection(InterfaceType = typeof(IEntityInfoRepository), Scope = InjectionScope.Singleton)]
    public class EntityInfoRepository : IEntityInfoRepository
    {
        private IEntityInfoStore _entityInfoStore;

        public EntityInfoRepository(IEntityInfoStore entityInfoStore)
        {
            _entityInfoStore = entityInfoStore;
        }
        public async  Task<EntityInfo> QueryByEntityType(string entityType)
        {
            return await _entityInfoStore.QueryByEntityType(entityType);
        }
    }
}
