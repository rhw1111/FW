using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Context
{
    /// <summary>
    /// 生成用于链路追踪的Http请求头
    /// </summary>
    public interface IGenerateTraceHttpHeadersService
    {
        Task<IDictionary<string, string>> Generate();
    }
}
