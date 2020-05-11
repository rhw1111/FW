using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求响应中间件
    /// </summary>
    public interface ISRRMiddleware
    {
        Task Invoke(ISRRPipeContext context);
    }
}
