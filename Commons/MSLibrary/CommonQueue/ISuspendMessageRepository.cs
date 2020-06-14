using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.CommonQueue
{
    public interface ISuspendMessageRepository
    {
        Task<SuspendMessage> QueryByFirstKey(string key);
        Task<SuspendMessage> QueryByID(Guid id,string key);
    }
}
