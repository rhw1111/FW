using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Configuration
{
    /// <summary>
    /// 系统配置服务
    /// 系统相关配置信息从该服务获取
    /// </summary>
    public interface ISystemConfigurationService
    {
        string GetConnectionString(string name, CancellationToken cancellationToken = default);
        Task<string> GetConnectionStringAsync(string name, CancellationToken cancellationToken = default);

        string GetHostApplicationName(CancellationToken cancellationToken = default);
        Task<string> GetHostApplicationNameAsync(CancellationToken cancellationToken = default);

        Guid GetDefaultUserID(CancellationToken cancellationToken = default);
        Task<Guid> GetDefaultUserIDAsync(CancellationToken cancellationToken = default);

        string[] GetApplicationCrosOrigin(CancellationToken cancellationToken = default);
        Task<string[]> GetApplicationCrosOriginAsync(CancellationToken cancellationToken = default);

    }
}
