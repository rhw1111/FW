using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.RemoteService
{
    public interface IRemoteServiceDescriptionRepository
    {
        Task<RemoteServiceDescription> QueryByName(string name);

        Task<RemoteServiceDescription> QueryByID(Guid id);

        Task<QueryResult<RemoteServiceDescription>> QueryByPage(string name, int page, int pageSize);
    }
}
