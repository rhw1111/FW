using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    public interface IIdentityHostStore
    {
        Task<IdentityHost?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
