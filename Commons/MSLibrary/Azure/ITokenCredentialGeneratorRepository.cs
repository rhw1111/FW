using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Azure
{
    public interface ITokenCredentialGeneratorRepository
    {
        Task<TokenCredentialGenerator> QueryByName(string name);
    }
}
