using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Cache;

namespace MSLibrary.MessageQueue
{
    public interface ISMessageExecuteTypeRepositoryCacheProxy
    {
        Task<SMessageExecuteType> QueryByName(string name);
    }
}
