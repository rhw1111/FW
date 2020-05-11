using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 请求处理描述
    /// </summary>
    public interface ISRRRequestHandlerDescription
    {
        IList<ISRRFilter> Filters { get; }
        IFactory<ISRRRequestHandler> HandlerFactory{ get; set; }
    }
}
