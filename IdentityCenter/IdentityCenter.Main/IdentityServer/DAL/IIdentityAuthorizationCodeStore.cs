using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    public interface IIdentityAuthorizationCodeStore
    {
        Task Add(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<IdentityAuthorizationCode?> QueryByCode(string code, CancellationToken cancellationToken = default);
    }
}
