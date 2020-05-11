using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken
{
    public interface ITokenControllerRepository
    {
        Task<TokenController> QueryByName(string name);
    }
}
