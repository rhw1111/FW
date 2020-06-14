using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.CommandLine.SSH
{
    public interface ISSHEndpointRepositoryCacheProxy
    {
        Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
