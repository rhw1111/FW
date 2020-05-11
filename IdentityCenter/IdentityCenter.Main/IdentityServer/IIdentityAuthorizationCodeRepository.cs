using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityAuthorizationCodeRepository
    {
        Task<IdentityAuthorizationCode?> QueryByCode(string code, CancellationToken cancellationToken = default);
    }
}
