using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MSLibrary;
using MSLibrary.Context;
using MSLibrary.DI;

namespace MSLibrary.Context.HttpExtensionContextHandleServices
{
    /// <summary>
    /// 针对链路追踪的Http扩展上下文处理
    /// 链路追踪信息来自request.Headers["rtraceid"],request.Headers["rtracelink"]
    /// </summary>
    [Injection(InterfaceType = typeof(HttpExtensionContextHandleServiceForTrace), Scope = InjectionScope.Singleton)]
    public class HttpExtensionContextHandleServiceForTrace : IHttpExtensionContextHandleService
    {
        public void GenerateContext(object info)
        {
            var realInfo = (RequestTraceInofContextDefault)info;

            ContextContainer.SetValue<IRequestTraceInofContext>(ContextTypes.Trace, realInfo);
        }

        public async Task<object> GetInfo(HttpRequest request)
        {
            string strTraceID = Guid.NewGuid().ToString();
            if (request.Headers.TryGetValue("rtraceid",out StringValues traceIDValue))
            {
                strTraceID = traceIDValue[0];
            }

            string strLinkID = "0";
            if (request.Headers.TryGetValue("rtracelink", out StringValues linkIDValue))
            {
                strLinkID = linkIDValue[0];
            }

            return await Task.FromResult(new RequestTraceInofContextDefault(strTraceID, strLinkID));
        }

             
    }


}
