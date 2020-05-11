using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityCenter.Main.IdentityServer.ClientBindings;

namespace IdentityCenter.Main.IdentityServer.ClientBindings
{
    public interface IIdentityClientOpenIDBindingRepository
    {
        Task<IdentityClientOpenIDBinding?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
