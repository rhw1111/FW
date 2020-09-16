using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Context
{
    [Injection(InterfaceType = typeof(IGenerateTraceHttpHeadersService), Scope = InjectionScope.Singleton)]
    public class GenerateTraceHttpHeadersService : IGenerateTraceHttpHeadersService
    {
        public async Task<IDictionary<string, string>> Generate()
        {
            Dictionary<string, string> headers;
            var traceInfoContext=ContextContainer.GetValue<IRequestTraceInofContext>(ContextTypes.Trace);
            if (traceInfoContext!=null)
            {
                headers = new Dictionary<string, string>() { { "rtraceid", traceInfoContext.GetTraceID() },{ "rtracelink",traceInfoContext.GetChildLinkID() } };
            }
            else
            {
                headers = new Dictionary<string, string>();
            }
            return headers;
        }
    }
}
