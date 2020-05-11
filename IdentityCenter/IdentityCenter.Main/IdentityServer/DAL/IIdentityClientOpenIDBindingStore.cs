using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityCenter.Main.IdentityServer.ClientBindings;


namespace IdentityCenter.Main.IdentityServer.DAL
{
    public interface IIdentityClientOpenIDBindingStore
    {
        Task<IdentityClientOpenIDBinding?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
