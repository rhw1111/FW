using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.RemoteService.DAL
{
    public interface IRemoteServiceDescriptionStore
    {
        Task Add(RemoteServiceDescription description);
        Task Update(RemoteServiceDescription description);
        Task Delete(Guid id);
        Task<RemoteServiceDescription> QueryByName(string name);

        Task<RemoteServiceDescription> QueryByID(Guid id);

        Task<QueryResult<RemoteServiceDescription>> QueryByPage(string name,int page,int pageSize);
    }
}
