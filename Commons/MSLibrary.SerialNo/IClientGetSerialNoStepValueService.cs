using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.SerialNo
{
    /// <summary>
    /// 客户端获取流水号步长值服务接口
    /// 需要实现该接口，用来完成与流水号服务的通信
    /// </summary>
    public interface IClientGetSerialNoStepValueService
    {
        Task<SerialNoRecordStepValue> Get(string configurationName, string prefix, CancellationToken cancellationToken = default);
    }
}
