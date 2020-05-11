using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SRR
{
    /// <summary>
    /// SSR主机配置
    /// </summary>
    public interface ISRRHostConfiguration
    {
        IDictionary<string, IList<ISRRMiddleware>> Middlewares { get; }
        IList<ISRRFilter> GlobalFilters { get; }

        void RegisterHandlerDescription(string requestType,IList<ISRRFilter> filters,IFactory<ISRRRequestHandler> handlerFactory);

        ISRRRequestHandlerDescription GetHandlerDescription(string requestType);
    }
}
