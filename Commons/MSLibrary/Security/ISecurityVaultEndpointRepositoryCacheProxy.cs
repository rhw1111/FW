using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security
{
    public interface ISecurityVaultEndpointRepositoryCacheProxy
    {
        Task<SecurityVaultEndpoint> QueryByName(string name);
    }
}
