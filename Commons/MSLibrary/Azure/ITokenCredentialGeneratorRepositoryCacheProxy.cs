using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Azure
{
    public interface ITokenCredentialGeneratorRepositoryCacheProxy
    {
        Task<TokenCredentialGenerator> QueryByName(string name);
    }
}
