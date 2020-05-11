using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security
{
    public interface ISecurityVaultEndpointRepository
    {
        Task<SecurityVaultEndpoint> QueryByName(string name);
    }
}
