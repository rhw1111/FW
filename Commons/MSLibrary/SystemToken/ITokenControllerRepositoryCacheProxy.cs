using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken
{
    public interface ITokenControllerRepositoryCacheProxy
    {
        Task<TokenController> QueryByName(string name);
    }
}
