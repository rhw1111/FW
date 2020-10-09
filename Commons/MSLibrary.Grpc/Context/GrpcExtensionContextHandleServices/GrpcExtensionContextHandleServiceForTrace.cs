using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Grpc.Context.GrpcExtensionContextHandleServices
{
    [Injection(InterfaceType = typeof(GrpcExtensionContextHandleServiceForTrace), Scope = InjectionScope.Singleton)]
    public class GrpcExtensionContextHandleServiceForTrace : IGrpcExtensionContextHandleService
    {
        public void GenerateContext(object info)
        {
            var realInfo = (RequestTraceInofContextDefault)info;
            ContextContainer.SetValue<IRequestTraceInofContext>(ContextTypes.Trace, realInfo);
        }

        public async Task<object> GetInfo(ServerCallContext callContext)
        {
            string strTraceID = Guid.NewGuid().ToString();
            string strLinkID = "0";
            var traceIDValue = callContext.RequestHeaders.Get("rtraceid");
            var linkIDValue= callContext.RequestHeaders.Get("rtracelink");

            if (traceIDValue!=null)
            {
                strTraceID = traceIDValue.Value;
            }

            if (linkIDValue != null)
            {
                strTraceID = linkIDValue.Value;
            }


            return await Task.FromResult(new RequestTraceInofContextDefault(strTraceID, strLinkID));
        }
    }
}
