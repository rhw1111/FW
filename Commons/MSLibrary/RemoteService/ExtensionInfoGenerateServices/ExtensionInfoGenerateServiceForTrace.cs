using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.RemoteService.ExtensionInfoGenerateServices
{
    /// <summary>
    /// 生成链路追踪上下文
    /// </summary>
    [Injection(InterfaceType = typeof(ExtensionInfoGenerateServiceForTrace), Scope = InjectionScope.Singleton)]
    public class ExtensionInfoGenerateServiceForTrace : IExtensionInfoGenerateService
    {
        public async Task<IDictionary<string, string>> Generate(string name, object state)
        {
            return await Task.FromResult(GenerateSync(name,state));
        }

        public IDictionary<string, string> GenerateSync(string name, object state)
        {
            Dictionary<string, string> headers;
            var traceInfoContext = ContextContainer.GetValue<IRequestTraceInofContext>(ContextTypes.Trace);
            if (traceInfoContext != null)
            {
                headers = new Dictionary<string, string>() { { "rtraceid", traceInfoContext.GetTraceID() }, { "rtracelink", traceInfoContext.GetChildLinkID() } };
            }
            else
            {
                headers = new Dictionary<string, string>();
            }
            return headers;
        }
    }
}
