using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.NetGateway
{
    /// <summary>
    /// 网关数据处理服务
    /// </summary>
    public interface INetGatewayDataHandleService
    {
        Task<INetGatewayDataHandleResult> Execute(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 网关数据处理服务的配置信息获取服务
    /// 涉及到要获取的配置信息，从该接口的方法获取
    /// </summary>
    public interface INetGatewayDataHandleConfigurationService
    {
        /// <summary>
        /// 获取要监控处理的文件目录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetDataFileFolderPath(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 从文件流中获取源数据字符串服务
    /// 一个文件流中有多个源数据字符串
    /// sourceDataAction针对每个源数据字符串做处理
    /// </summary>
    public interface IGetSourceDataFromStreamService
    {
        /// <summary>
        /// 获取源数据字符串
        /// sourceDataAction中可以得到从流中获取的每个源数据字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="dataformat"></param>
        /// <param name="sourceDataAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Get(Stream stream, string dataformat, Func<string, Task> sourceDataAction, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 从文件流中获取源数据字符串服务
    /// 一个文件流中有多个源数据字符串
    /// sourceDataAction针对每个源数据字符串做处理
    /// </summary>
    public interface IGetSourceDataFromFileService
    {
        /// <summary>
        /// 获取源数据字符串
        /// sourceDataAction中可以得到从流中获取的每个源数据字符串
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataformat"></param>
        /// <param name="sourceDataAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Get(string fileName, string dataformat, Func<string, Task> sourceDataAction, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 获取文件名称的前缀
    /// 在当前场景中，该前缀为TestCaseHistory的ID
    /// </summary>
    public interface IResolveFileNamePrefixService
    {
        Task<TestCaseHistory?> Resolve(string fileName);
    }

    /// <summary>
    /// 源数据字符串转换为自定义的网络数据
    /// 自定义的网络数据用于计算各种指标
    /// </summary>
    public interface IConvertNetDataFromSourceService
    {
        Task<NetData?> Convert(string prefix,string sourceData, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// QPS指标收集服务
    /// 当前场景中，将传递该指标到流式数据库
    /// </summary>
    public interface IQPSCollectService
    {
        Task Collect(string prefix,int qps,DateTime time, CancellationToken cancellationToken = default);
    }
    /// <summary>
    /// 延迟指标收集服务
    ///  当前场景中，将传递该指标到流式数据库
    /// </summary>
    public interface INetDurationCollectService
    {
        Task Collect(string prefix, double min,double max,double avg, DateTime time, CancellationToken cancellationToken = default);
    }

    public enum NetDataType
    {
        Request,
        Response
    }
    public class NetData
    {
        public NetDataType Type { get; set; }
        public string ID { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public int? RunDuration { get; set; }
    }

    public interface INetGatewayDataHandleResult
    {
        Task Stop();
    }


}
