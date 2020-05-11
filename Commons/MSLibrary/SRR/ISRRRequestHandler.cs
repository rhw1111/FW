using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息处理接口
    /// </summary>
    public interface ISRRRequestHandler
    {
        Task<SRRResponse> Execute(SRRRequest request);
    }
}
