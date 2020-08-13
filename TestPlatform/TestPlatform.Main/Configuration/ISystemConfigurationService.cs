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

        /// <summary>
        /// 获取CaseService服务的基地址
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        string GetCaseServiceBaseAddress(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取CaseService服务的基地址（异步）
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetCaseServiceBaseAddressAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定引擎类型的监控地址
        /// </summary>
        /// <param name="enginType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        string GetMonitorAddress(string enginType, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定引擎类型的监控地址（异步）
        /// </summary>
        /// <param name="enginType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetMonitorAddressAsync(string enginType, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取测试历史记录的监控地址
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        string GetTestCaseHistoryMonitorAddress(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定引擎类型的测试历史记录监控地址（异步）
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetCaseHistoryMonitorAddressAsync(CancellationToken cancellationToken = default);
    }
}
