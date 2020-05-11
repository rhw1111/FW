using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Storge
{
    public interface IStoreGroupRepositoryCacheProxy
    {
        Task<StoreGroup> QueryByName(string name);
    }
}
