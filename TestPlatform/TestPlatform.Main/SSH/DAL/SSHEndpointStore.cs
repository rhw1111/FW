using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.CommandLine.SSH;
using MSLibrary.CommandLine.SSH.DAL;
using MSLibrary.DI;
using Microsoft.EntityFrameworkCore;
using MSLibrary.Transaction;
using FW.TestPlatform.Main.DAL;

namespace FW.TestPlatform.Main.SSH.DAL
{

    [Injection(InterfaceType = typeof(ISSHEndpointStore), Scope = InjectionScope.Singleton)]
    public class SSHEndpointStore : ISSHEndpointStore
    {
        public Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
