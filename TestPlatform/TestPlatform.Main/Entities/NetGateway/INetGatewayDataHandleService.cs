using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.NetGateway
{
    public interface INetGatewayDataHandleService
    {
        Task<INetGatewayDataHandleResult> Execute(CancellationToken cancellationToken = default);
    }

    public interface INetGatewayDataHandleConfigurationService
    {
        Task<string> GetDataFileFolderPath(CancellationToken cancellationToken = default);
    }

    public interface IGetSourceDataFromStreamService
    {
        Task Get(Stream stream,Func<string,Task> sourceDataAction, CancellationToken cancellationToken = default);
    }

    public interface IResolveFileNamePrefixService
    {
        string Resolve(string fileName);
    }

    public interface IConvertNetDataFromSourceService
    {
        Task<NetData> Convert(string sourceData, CancellationToken cancellationToken = default);
    }

    public interface IQPSCollectService
    {
        Task Collect(int qps,DateTime time, CancellationToken cancellationToken = default);
    }

    public interface INetDurationCollectService
    {
        Task Collect(double min,double max,double avg, DateTime time, CancellationToken cancellationToken = default);
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
