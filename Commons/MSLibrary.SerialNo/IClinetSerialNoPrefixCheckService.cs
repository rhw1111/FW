using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.SerialNo
{
    /// <summary>
    /// 客户端流水号前缀检查服务
    /// 如果返回结果为false
    /// 则将在缓存中移除该前缀
    /// </summary>
    public interface IClinetSerialNoPrefixCheckService
    {
        Task<bool> Check(string configurationName,string prefix, CancellationToken cancellationToken = default);
    }
}
