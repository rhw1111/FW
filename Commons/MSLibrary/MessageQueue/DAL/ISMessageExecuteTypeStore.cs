using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 消息处理类型数据操作
    /// </summary>
    public interface ISMessageExecuteTypeStore
    {
        Task Add(SMessageExecuteType messageType);
        Task Update(SMessageExecuteType messageType);
        Task Delete(Guid id);

        Task<SMessageExecuteType> QueryById(Guid id);
        Task<SMessageExecuteType> QueryByName(string name);
        Task<QueryResult<SMessageExecuteType>> Query(string name,int page,int pageSize);
    }
}
