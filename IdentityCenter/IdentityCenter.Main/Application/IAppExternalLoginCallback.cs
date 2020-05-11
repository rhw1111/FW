using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using IdentityCenter.Main.DTOModel;

namespace IdentityCenter.Main.Application
{
    public interface IAppExternalLoginCallback
    {
        Task<ExternalLoginCallbackResult> Do(AuthenticateResult authenticateResult, CancellationToken cancellationToken = default);
    }
}
