using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Cache;

namespace MSLibrary.Context
{
    public interface IClaimContextGeneratorRepositoryCacheProxy
    {
        Task<ClaimContextGenerator> QueryByName(string name);
    }
}
