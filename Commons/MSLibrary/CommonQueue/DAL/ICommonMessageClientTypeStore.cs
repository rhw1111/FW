using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.CommonQueue.DAL
{
    public interface ICommonMessageClientTypeStore
    {
        Task Add(CommonMessageClientType type);
        Task Update(CommonMessageClientType type);
        Task Delete(Guid id);
        Task<CommonMessageClientType> QueryByID(Guid id);
        Task<CommonMessageClientType> QueryByName(string name);
        Task<QueryResult<CommonMessageClientType>> QueryByPage(string name, int page, int pageSize);
    }
}
