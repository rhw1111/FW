using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求响应主机服务
    /// </summary>
    public interface ISRRHostService
    {
        void Configure(ISRRHostConfiguration configuration);
        Task<SRRResponse> Handle(string sourceType, IDictionary<string, object> sourceParameters, SRRRequest request);
    }
}
