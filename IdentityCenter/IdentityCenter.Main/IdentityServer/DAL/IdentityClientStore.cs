using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityCenter.Main.Entities.DAL;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.Collections;
using IdentityCenter.Main.DAL;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    [Injection(InterfaceType = typeof(IIdentityClientStore), Scope = InjectionScope.Singleton)]
    public class IdentityClientStore : IIdentityClientStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityClientStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task<IdentityClient?> QueryByClientID(string clientID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
