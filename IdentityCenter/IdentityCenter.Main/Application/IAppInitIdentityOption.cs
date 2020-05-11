using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Application
{
    public interface IAppInitIdentityOption
    {
        Task Do<T>(string schemeName,T options, CancellationToken cancellationToken = default);
    }
}
