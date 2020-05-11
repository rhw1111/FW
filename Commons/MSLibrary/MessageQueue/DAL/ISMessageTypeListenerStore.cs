using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 消息监听者数据操作
    /// </summary>
    public interface ISMessageTypeListenerStore
    {
        Task Add(SMessageTypeListener listener);
        Task UpdateByTypeRelation(SMessageTypeListener listener);

        Task Delete(Guid id);
        Task DeleteByTypeRelation(Guid typeId,Guid listenerId);

        Task<SMessageTypeListener> QueryById(Guid id);

        Task<SMessageTypeListener> QueryByTypeRelation(Guid typeId,Guid id);

        Task QueryByType(Guid typeId, Func<SMessageTypeListener, Task> callback);
        Task<QueryResult<SMessageTypeListener>> QueryByType(Guid typeId, int page, int pageSize);





    }
}
