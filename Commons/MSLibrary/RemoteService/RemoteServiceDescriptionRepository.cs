using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.RemoteService.DAL;

namespace MSLibrary.RemoteService
{
    [Injection(InterfaceType = typeof(IRemoteServiceDescriptionRepository), Scope = InjectionScope.Singleton)]
    public class RemoteServiceDescriptionRepository : IRemoteServiceDescriptionRepository
    {
        private IRemoteServiceDescriptionStore _remoteServiceDescriptionStore;

        public RemoteServiceDescriptionRepository(IRemoteServiceDescriptionStore remoteServiceDescriptionStore)
        {
            _remoteServiceDescriptionStore = remoteServiceDescriptionStore;
        }
        public async Task<RemoteServiceDescription> QueryByID(Guid id)
        {
            return await _remoteServiceDescriptionStore.QueryByID(id);
        }

        public async Task<RemoteServiceDescription> QueryByName(string name)
        {
            return await _remoteServiceDescriptionStore.QueryByName(name);
        }

        public async Task<QueryResult<RemoteServiceDescription>> QueryByPage(string name, int page, int pageSize)
        {
            return await _remoteServiceDescriptionStore.QueryByPage(name, page, pageSize);
        }
    }
}
