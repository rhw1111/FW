using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context
{
    public interface IEnvironmentClaimGeneratorRepositoryCacheProxy
    {
        Task<EnvironmentClaimGenerator> QueryByName(string name);
    }
}
