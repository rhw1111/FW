using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.CommonQueue
{
    public interface ICommonMessageClientTypeRepositoryCacheProxy
    {
        Task<CommonMessageClientType> QueryByName(string name);
    }
}
