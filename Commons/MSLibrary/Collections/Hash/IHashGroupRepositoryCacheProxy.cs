using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections.Hash
{
    public interface IHashGroupRepositoryCacheProxy
    {
        Task<HashGroup> QueryById(Guid id);
        Task<HashGroup> QueryByName(string name);
        HashGroup QueryByNameSync(string name);
    }
}
