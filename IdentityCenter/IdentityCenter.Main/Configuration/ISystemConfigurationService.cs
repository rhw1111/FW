using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.Configuration
{
    public interface ISystemConfigurationService
    {
        string GetConnectionString(string name, CancellationToken cancellationToken = default);
        Task<string> GetIdentityHostApplicationName(CancellationToken cancellationToken = default);
        Task<string> GetIdentityClientApplicationName(CancellationToken cancellationToken = default);
    }
}
