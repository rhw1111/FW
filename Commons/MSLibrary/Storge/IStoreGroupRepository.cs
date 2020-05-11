using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Storge
{
    public interface IStoreGroupRepository
    {
        Task<StoreGroup> QueryByName(string name);
    }
}
