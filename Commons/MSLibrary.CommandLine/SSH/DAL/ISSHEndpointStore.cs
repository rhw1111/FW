using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.CommandLine.SSH.DAL
{
    /// <summary>
    /// SSH终结点数据操作
    /// </summary>
    public interface ISSHEndpointStore
    {
        Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
